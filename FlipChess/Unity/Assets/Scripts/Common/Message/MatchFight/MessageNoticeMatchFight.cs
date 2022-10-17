using MessagePack;

namespace GameCommon
{
    public class MessageNoticeMatchFight : AMessageNotice
    {
        [Key(1)]
        public int FightId { get; set; }
    }
}
