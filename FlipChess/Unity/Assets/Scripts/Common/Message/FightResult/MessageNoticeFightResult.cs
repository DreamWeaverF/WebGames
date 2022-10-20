using MessagePack;

namespace GameCommon
{
    public class MessageNoticeFightResult : AMessageNotice
    {
        [Key(1)]
        public long WinUserId { get; set; }
        [Key(2)]
        public FightResultType fightResultType { get; set; }
    }
}
