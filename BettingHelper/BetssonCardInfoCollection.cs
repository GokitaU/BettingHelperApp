using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingHelper
{
    class BetssonPublishedCardInfoCollection
    {

        public List<BetssonCardInformation> GetPublishedCardsResult;

        public BetssonCardInformation LatestCard => GetPublishedCardsResult.OrderBy(e => e.DeadlineDate).FirstOrDefault();
    }
}
