using MessagePack;
using System.Collections.Generic;

namespace GameCommon
{
    public class MessageResponseRankData : AMessageResponse
    {
        [Key(1)]
        public List<RankDataElement> RankDataElements;
    }
}
