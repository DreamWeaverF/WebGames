using MessagePack;

namespace GameCommon
{
    public class MessageRequestJoinFight : AMessageRequest
    {
        [Key(1)]
        public int FightId { get; set; }
    }
}
