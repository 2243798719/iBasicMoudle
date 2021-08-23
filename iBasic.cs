using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iBasicMoudle
{
    class Versions
    {
        public readonly string url = "https://launchermeta.mojang.com/mc/game/version_manifest.json";
        public Task<VerInfor[]> GetVersionsArray()
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            var VJson = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            JObject VJson_ = JObject.Parse(VJson);
            JArray _VJson = JArray.Parse(VJson_["versions"].ToString());
            VerInfor[] vinf = new VerInfor[_VJson.Count];

            for (int i = 0; i < _VJson.Count; i++)
            {
                vinf[i].version = _VJson[i]["id"].ToString();
                vinf[i].releaseTime = _VJson[i]["releaseTime"].ToString();
                vinf[i].type = _VJson[i]["type"].ToString();
                vinf[i].url = _VJson[i]["url"].ToString();
            }

            return Task.FromResult(vinf);
        }

        public Task VersionsJsonDownload(string path ,string version)
        {
            string _url = "https://bmclapi2.bangbang93.com/version/" + version + "/json";
            string JsonPath = path + "\\" + version + ".json";
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(_url);
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            string json = new StreamReader(Response.GetResponseStream()).ReadToEnd();
            //if (!Directory.Exists(JsonPath)) { Directory.CreateDirectory(JsonPath); }
            FileStream Vjson = new FileStream(JsonPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(Vjson);
            sw.Write(json);
            sw.Flush();
            sw.Close();
            /*File.Create
            File.Create("D:\\.test\\" + Temp1[listBox1.SelectedIndex].version.ToString() + "\\.json");
            await File.WriteAllTextAsync("D:\\.test\\" + Temp1[listBox1.SelectedIndex].version.ToString() + "\\.json", json);*/

            return null;
        }
    }

    public struct VerInfor
    {
        public string version;
        public string releaseTime;
        public string type;
        public string url;
    }
}
