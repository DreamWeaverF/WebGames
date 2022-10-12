using MessagePack;

namespace GameCommon
{
    [MessagePackObject]
    public abstract class AMessageResponse : IMessage
    {
        [Key(100)]
        public int RpcId { get; set; }
        [Key(101)]
        public MessageErrorCode ErrorCode { get; set; }
    }
}
