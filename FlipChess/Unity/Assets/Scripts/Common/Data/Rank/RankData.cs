using MessagePack;
using System.Collections.Generic;

namespace GameCommon
{
    [MessagePackObject]
    public class RankDataElement 
    {
        [Key(1)]
        public long UserId { get; set; }
        [Key(2)]
        public string UserNick { get; set; }
        [Key(3)]
        public string UserHeadIcon { get; set; }
        [Key(4)]
        public int UserScore { get; set; }
    }

    [MessagePackObject]
    public class RankData
    {
        [Key(1)]
        public int RankId { get; set; }
        [Key(2)]
        public List<RankDataElement> RankDataElements { get; set; }
    }
}
