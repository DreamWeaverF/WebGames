using MessagePack;

namespace GameCommon
{
    public class MessageNoticeMatchFight : AMessageNotice
    {
        [Key(1)]
        public int FightId { get; set; }
        [Key(2)]
        public long UserId1 { get; set; }
        [Key(3)]
        public long UserId2 { get; set; }
    }
}
