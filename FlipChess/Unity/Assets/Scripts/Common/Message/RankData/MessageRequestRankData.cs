using MessagePack;

namespace GameCommon
{
    public class MessageRequestRankData : AMessageRequest
    {
        [Key(1)]
        public int RankId { get; set; }
    }
}
