using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace GithubHelper
{
    public class GithubUpdater
    {
        private string UserName = null;
        private string Repository = null;

        private static string GithubAPI = "https://api.github.com";

        private static readonly HttpClient client = new HttpClient();

        private struct Asset
        {
            public string name;
            public string type;
            public string DownloadURL;
            public Version version;
        }
        private readonly object stateLock = new object();
        private JsonElement? RepoData;
        private List<Asset> Assets = new List<Asset>();

        private Asset Firmware = new Asset();
        private Asset App = new Asset();

        public string FirmwareVersion
            {
                get {
                lock (stateLock)
                {
                    if (Firmware.version == null) {
                        return "0.0.0";

                    }
                    return Firmware.version.ToString();
                }
            }
            }
        public string AppVersion
            {
                get
            {
                lock (stateLock)
                {
                    if (App.version == null)
                    {
                        return "0.0.0.0";
                    }
                    return App.version.ToString();
                }
            }
            }
        public string FirmwareFileName
        {
            get { lock (stateLock) { return Firmware.name; } }
        }

        public string AppFileName
        {
            get { lock (stateLock) { return App.name; } }
        }
        public string Changelog
        {
            get
            {
                string htmlUrl = GetReleaseString("html_url");
                if (htmlUrl != null)
                {
                    return htmlUrl;
                }
                return "https://github.com/"+ UserName+"/"+ Repository +"/releases/latest";
            }

        }

        public string CurrentRelease
        {
            get { return GetReleaseString("tag_name"); }
        }

        public GithubUpdater(string User, string Repo)
        {
            if ((User != null) && (User != ""))
            {
                UserName = User;
            }
            if ((Repo != null) && (Repo != ""))
            {
                Repository = Repo;
            }

        }

        public bool Update()
        {
            if ((UserName != null) && (Repository != null))
            {

                bool fetched = GetData();
                bool hasData;
                lock (stateLock)
                {
                    hasData = RepoData != null;
                }
                if (fetched && hasData)
                {
                    GetAssets();
                    return true;
                }
            }
            return false;
        }

        private HttpRequestMessage CreateRequest(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.TryAddWithoutValidation("User-Agent", Repository + "_UpdateClient");
            return request;
        }

        private static string GetString(JsonElement element, string propertyName)
        {
            if ((element.ValueKind == JsonValueKind.Object) &&
                element.TryGetProperty(propertyName, out JsonElement value) &&
                (value.ValueKind == JsonValueKind.String))
            {
                return value.GetString();
            }
            return null;
        }

        private string GetReleaseString(string propertyName)
        {
            lock (stateLock)
            {
                if (RepoData != null)
                {
                    return GetString(RepoData.Value, propertyName);
                }
                return null;
            }
        }

        private bool GetData()
        {
            string URL = String.Join("/", new string[] { @GithubAPI, "repos", @UserName, @Repository, "releases/latest" });
            try
            {
                using (HttpRequestMessage request = CreateRequest(URL))
                using (HttpResponseMessage response = client.Send(request))
                {
                    response.EnsureSuccessStatusCode();
                    using (Stream stream = response.Content.ReadAsStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        using (JsonDocument document = JsonDocument.Parse(json))
                        {
                            JsonElement root = document.RootElement.Clone();
                            lock (stateLock)
                            {
                                RepoData = root;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // Offline, HTTP error or malformed response: keep the last known
                // release data but report this check as unsuccessful.
                System.Diagnostics.Trace.WriteLine("Update check failed: " + ex.Message);
                return false;
            }
        }



        private void GetAssets()
        {
            lock (stateLock)
            {
                Assets.Clear();
                if ((RepoData != null) &&
                    (RepoData.Value.ValueKind == JsonValueKind.Object) &&
                    RepoData.Value.TryGetProperty("assets", out JsonElement assetsElement) &&
                    (assetsElement.ValueKind == JsonValueKind.Array))
                {
                    foreach (JsonElement asset in assetsElement.EnumerateArray())
                    {
                        Assets.Add(new Asset() { name = GetString(asset, "name"), DownloadURL = GetString(asset, "browser_download_url"), type = GetString(asset, "content_type") });
                    }
                }
                Firmware = new Asset();
                App = new Asset();
                Asset? firmware = Assets.Where(a => (a.name != null) && Path.GetExtension(a.name).Equals(".hex", StringComparison.OrdinalIgnoreCase)).Select(a => (Asset?)a).FirstOrDefault();
                if (firmware != null)
                {
                    Asset found = firmware.Value;
                    if (Version.TryParse(found.name.Replace(".hex", "").Replace(Repository + "_", ""), out Version version))
                    {
                        found.version = version;
                    }
                    Firmware = found;
                }
                // Prefer the Burn bootstrapper (.exe), which installs the .NET 10
                // Desktop Runtime before the MSI. Older releases only have an .msi,
                // so fall back to that. Both are named fcc_controller_<version>.<ext>.
                Asset? app = Assets.Where(a => (a.name != null) && a.name.StartsWith(Repository + "_", StringComparison.Ordinal) && Path.GetExtension(a.name).Equals(".exe", StringComparison.OrdinalIgnoreCase)).Select(a => (Asset?)a).FirstOrDefault();
                if (app == null)
                {
                    app = Assets.Where(a => (a.name != null) && Path.GetExtension(a.name).Equals(".msi", StringComparison.OrdinalIgnoreCase)).Select(a => (Asset?)a).FirstOrDefault();
                }
                if (app != null)
                {
                    Asset found = app.Value;
                    string assetName = Path.GetFileNameWithoutExtension(found.name);
                    if (Version.TryParse(assetName.Replace(Repository + "_", ""), out Version version))
                    {
                        found.version = version;
                    }
                    App = found;
                }
            }
        }



        public bool CheckFirmware(string CurrentVersion)
        {
            lock (stateLock)
            {
                if (Firmware.version == null)
                {
                    return false;
                }
                if (Version.TryParse(CurrentVersion, out Version currentVersion) &&
                    (currentVersion < Firmware.version))
                {
                    return true;
                }

                return false;
            }
        }

        public bool CheckApp(string CurrentVersion)
        {
            lock (stateLock)
            {
                if (App.version == null)
                {
                    return false;
                }
                if (Version.TryParse(CurrentVersion, out Version currentVersion) &&
                    (currentVersion < App.version))
                {
                    return true;
                }

                return false;
            }
        }

        public bool DownloadFirmware(string TargetPath)
        {
            string name;
            string url;
            lock (stateLock)
            {
                name = Firmware.name;
                url = Firmware.DownloadURL;
            }
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(TargetPath))
            {
                return false;
            }
            if (!Directory.Exists(TargetPath))
            {
                return false;
            }

            try
            {
                if (Path.GetFileName(name) != name)
                {
                    return false;
                }
                using (HttpRequestMessage request = CreateRequest(url))
                using (HttpResponseMessage response = client.Send(request))
                {
                    response.EnsureSuccessStatusCode();
                    using (Stream content = response.Content.ReadAsStream())
                    using (FileStream file = File.Create(Path.Combine(TargetPath, name)))
                    {
                        content.CopyTo(file);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Firmware download failed: " + ex.Message);
                return false;
            }
        }

        public bool DownloadApp(string TargetPath)
        {
            string name;
            string url;
            lock (stateLock)
            {
                name = App.name;
                url = App.DownloadURL;
            }
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(TargetPath))
            {
                return false;
            }
            if (!Directory.Exists(TargetPath))
            {
                return false;
            }

            try
            {
                if (Path.GetFileName(name) != name)
                {
                    return false;
                }
                using (HttpRequestMessage request = CreateRequest(url))
                using (HttpResponseMessage response = client.Send(request))
                {
                    response.EnsureSuccessStatusCode();
                    using (Stream content = response.Content.ReadAsStream())
                    using (FileStream file = File.Create(Path.Combine(TargetPath, name)))
                    {
                        content.CopyTo(file);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("App download failed: " + ex.Message);
                return false;
            }
        }
    }
}
