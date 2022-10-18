
using MessagePack;

namespace GameCommon
{
    public class MessageRequestFightChat : AMessageRequest
    {
        [Key(1)]
        public string Context { get; set; }
    }
}
