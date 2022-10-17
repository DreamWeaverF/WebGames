
using MessagePack;

namespace GameCommon
{
    public class MessageRequestChatMessage : AMessageRequest
    {
        [Key(1)]
        public string Context { get; set; }
    }
}
