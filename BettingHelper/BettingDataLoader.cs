using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace BettingHelper
{
    class BettingDataLoader
    {
        string _excelPath;
        string _betssonAPIPublishedCardUrl = "https://sbfacade.bpsgameserver.com/1x2FacadeServices/1x2FacadeService.svc/GetPublishedCard";
        string _betssonAPICardDetailsBaseURL = "https://sbfacade.bpsgameserver.com/1x2FacadeServices/1x2FacadeService.svc/GetCardDetailsCms?";
        string _svsAPIDataUrl = "https://api.www.svenskaspel.se/search/query/?ctx=draw&type=stryktipset,topptipset,europatipset&sort=payload.draw.regCloseTime&rangefilter=payload.draw.regCloseTime;gt;now&offset=0&count=40&hours=0";
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

        public async Task<List<IBettingEvent>> DownloadBetssonOdds()
        {
            var ids = await DownloadBetssonCardIds();
            BetssonCardInformation latestCard = ids.LatestCard;
            if(latestCard == null)
            {
                return new List<IBettingEvent>();
            }
            return await DownloadBetssonCardData(latestCard.Id);
        }

        private async Task<BetssonPublishedCardInfoCollection> DownloadBetssonCardIds()
        {
                HttpResponseMessage msg = await client.GetAsync(_betssonAPIPublishedCardUrl);
                msg.EnsureSuccessStatusCode();
                string result = await msg.Content.ReadAsStringAsync();
                BetssonPublishedCardInfoCollection deserialized = JsonConvert.DeserializeObject<BetssonPublishedCardInfoCollection>(result);
                return deserialized;
        }

        private async Task<List<IBettingEvent>> DownloadBetssonCardData(string cardId)
        {
            try
            {
                HttpResponseMessage msg = await client.GetAsync($"{_betssonAPICardDetailsBaseURL}id={cardId}&userid=0");
                msg.EnsureSuccessStatusCode();
                string result = await msg.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(result);
                IList<JToken> jsonList = json["GetCardDetailsCmsResult"]["Card"]["Events"].Children().ToList();
                List<IBettingEvent> events = new List<IBettingEvent>();
                foreach (var token in jsonList)
                {
                    BetssonEvent evt = token.ToObject<BetssonEvent>();
                    events.Add(evt);
                }
                return events;
            }catch(Exception e)
            {
                return null;
            }
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
                evts[draw.ProductName] = draw;
            }
            return evts;
        }


    }
}
