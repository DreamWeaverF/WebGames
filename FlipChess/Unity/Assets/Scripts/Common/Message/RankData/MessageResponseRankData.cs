using MessagePack;

namespace GameCommon
{
    public class MessageResponseRankData : AMessageResponse
    {
        [Key(1)]
        public RankData RankData;
    }
}
