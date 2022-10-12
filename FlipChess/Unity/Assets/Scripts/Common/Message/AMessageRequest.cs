using MessagePack;

namespace GameCommon
{
    [MessagePackObject]
    public abstract class AMessageRequest : IMessage
    {
        [Key(100)]
        public int RpcId { get; set; }
    }
}
