using MessagePack;

namespace GameCommon
{
    [MessagePackObject]
    public class RankData 
    {
        [Key(1)]
        public long UserId { get; set; }
        [Key(2)]
        public string UserNick { get; set; }
        [Key(3)]
        public string UserHeadIcon { get; set; }
        [Key(4)]
        public string UserScore { get; set; }
    }
}