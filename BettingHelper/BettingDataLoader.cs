using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace BettingHelper
{
    class BettingDataLoader
    {
        string _excelPath;
        string _svsAPIDataUrl = "https://api.www.svenskaspel.se/search/query/?ctx=draw&type=stryktipset,topptipset,europatipset&sort=payload.draw.regCloseTime&rangefilter=payload.draw.regCloseTime;gt;now&offset=0&count=4&hours=0";
        HttpClient client;

        public BettingDataLoader(string excelPath)
        {
            if (!File.Exists(excelPath))
            {
                throw new IOException($"Kan inte hitta fil med sökväg {excelPath}!");
            }
            _excelPath = excelPath;
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<Dictionary<string,SvenskaSpelDraw>> DownloadSvenskaSpelData()
        {
            HttpResponseMessage msg = await client.GetAsync(_svsAPIDataUrl);
            msg.EnsureSuccessStatusCode();
            string result = await msg.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(result);
            IList<JToken> jsonList = json["result"].Children().ToList();
            Dictionary<string,SvenskaSpelDraw> evts = new Dictionary<string,SvenskaSpelDraw>();
            foreach(JToken token in jsonList)
            {
                SvenskaSpelDraw draw = token["payload"]["draw"].ToObject<SvenskaSpelDraw>();
                if (!evts.ContainsKey(draw.ProductName))
                {
                    evts[draw.ProductName] = draw;
                }
            }
            return evts;
        }


    }
}
