using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;

namespace GithubHelper
{
    public class GithubUpdater
    {
        private string UserName = null;
        private string Repository = null;

        private static string GithubAPI = "https://api.github.com";

        private struct Asset
        {
            public string name;
            public string type;
            public string DownloadURL;
            public Version version;
        }
        private dynamic RepoData;
        private List<Asset> Assets = new List<Asset>();

        private Asset Firmware = new Asset();
        private Asset App = new Asset();

        public string FirmwareVersion
            {
                get { return Firmware.version.ToString(); }
            }
        public string AppVersion
            {
                get { return App.version.ToString(); }
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
            get { return RepoData[0]["html_url"]; }
        }

        public string CurrentRelease
        {
            get { return RepoData[0]["tag_name"]; }
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

        private void GetData()
        {
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", Repository + "_UpdateClient");
            string URL = String.Join("/", new string[] { @GithubAPI, "repos", @UserName, @Repository, "releases" });
            var json = wc.DownloadString(@URL);

            var jss = new JavaScriptSerializer();
            RepoData = jss.Deserialize<dynamic>(json);
        }



        private void GetAssets()
        {
            foreach (var asset in RepoData[0]["assets"])
            {
                Assets.Add(new Asset() { name = asset["name"], DownloadURL = asset["browser_download_url"], type = asset["content_type"] });
            }
            Firmware = Assets.Where(a => a.name.Contains(".hex")).First();
            Firmware.version = new Version(Firmware.name.Replace(".hex", "").Replace(Repository + "_", ""));
            App = Assets.Where(a => a.name.Contains(".msi")).First();
            App.version = new Version(App.name.Replace(".msi", "").Replace(Repository + "_", ""));

        }



        public bool CheckFirmware(string CurrentVersion)
        {
            if (new Version(CurrentVersion) < Firmware.version)
            {
                return true;
            }

            return false;
        }

        public bool CheckApp(string CurrentVersion)
        {
            if (new Version(CurrentVersion) < App.version)
            {
                return true;
            }

            return false;
        }

        public bool DownloadFirmware(string TargetPath)
        {
            if (!Directory.Exists(TargetPath))
            {
                return false;
            }

            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", Repository + "_UpdateClient");
            wc.DownloadFile(Firmware.DownloadURL, TargetPath + "\\" + Firmware.name);
            return true;
        }

        public bool DownloadApp(string TargetPath)
        {
            if (!Directory.Exists(TargetPath))
            {
                return false;
            }

            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", Repository + "_UpdateClient");
            wc.DownloadFile(App.DownloadURL, TargetPath + "\\" + App.name);
            return true;
        }
    }
}
