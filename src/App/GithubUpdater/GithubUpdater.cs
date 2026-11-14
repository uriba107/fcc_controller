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
        private JsonElement? RepoData;
        private List<Asset> Assets = new List<Asset>();

        private Asset Firmware = new Asset();
        private Asset App = new Asset();

        public string FirmwareVersion
            {
                get {
                if (Firmware.version == null) {
                    return "0.0.0";

                }
                return Firmware.version.ToString();
            }
            }
        public string AppVersion
            {
                get
            {
                if (App.version == null)
                {
                    return "0.0.0.0";
                }
                return App.version.ToString();
            }
            }
        public string FirmwareFileName
        {
            get { return Firmware.name; }
        }

        public string AppFileName
        {
            get { return App.name;  }
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

                GetData();
                if (RepoData != null)
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
            if (RepoData != null)
            {
                return GetString(RepoData.Value, propertyName);
            }
            return null;
        }

        private void GetData()
        {
            string URL = String.Join("/", new string[] { @GithubAPI, "repos", @UserName, @Repository, "releases/latest" });
            try
            {
                using (HttpRequestMessage request = CreateRequest(URL))
                using (HttpResponseMessage response = client.Send(request))
                {
                    response.EnsureSuccessStatusCode();
                    string json = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    using (JsonDocument document = JsonDocument.Parse(json))
                    {
                        RepoData = document.RootElement.Clone();
                    }
                }
            }
            catch
            { }
        }



        private void GetAssets()
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
            Asset? firmware = Assets.Where(a => (a.name != null) && a.name.Contains(".hex")).Select(a => (Asset?)a).FirstOrDefault();
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
            Asset? app = Assets.Where(a => (a.name != null) && a.name.StartsWith(Repository + "_") && a.name.Contains(".exe")).Select(a => (Asset?)a).FirstOrDefault();
            if (app == null)
            {
                app = Assets.Where(a => (a.name != null) && a.name.Contains(".msi")).Select(a => (Asset?)a).FirstOrDefault();
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



        public bool CheckFirmware(string CurrentVersion)
        {
            if (Firmware.version == null)
            {
                return false;
            }
            if (new Version(CurrentVersion) < Firmware.version)
            {
                return true;
            }

            return false;
        }

        public bool CheckApp(string CurrentVersion)
        {
            if (App.version == null)
            {
                return false;
            }
            if (new Version(CurrentVersion) < App.version)
            {
                return true;
            }

            return false;
        }

        public bool DownloadFirmware(string TargetPath)
        {
            if ((Firmware.DownloadURL == null) || (Firmware.name == null))
            {
                return false;
            }
            if (!Directory.Exists(TargetPath))
            {
                return false;
            }

            using (HttpRequestMessage request = CreateRequest(Firmware.DownloadURL))
            using (HttpResponseMessage response = client.Send(request))
            {
                response.EnsureSuccessStatusCode();
                byte[] data = response.Content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                File.WriteAllBytes(TargetPath + "\\" + Firmware.name, data);
            }
            return true;
        }

        public bool DownloadApp(string TargetPath)
        {
            if ((App.DownloadURL == null) || (App.name == null))
            {
                return false;
            }
            if (!Directory.Exists(TargetPath))
            {
                return false;
            }

            using (HttpRequestMessage request = CreateRequest(App.DownloadURL))
            using (HttpResponseMessage response = client.Send(request))
            {
                response.EnsureSuccessStatusCode();
                byte[] data = response.Content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                File.WriteAllBytes(TargetPath + "\\" + App.name, data);
            }
            return true;
        }
    }
}
