using MessagePack;

namespace GameCommon
{
    public class MessageResponseLogin : AMessageResponse
    {
        [Key(1)]
        public long UserId { get; set; }
    }
}
