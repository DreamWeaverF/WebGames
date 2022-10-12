using MessagePack;

namespace GameCommon
{
    public class MessageRequestLogin : AMessageRequest
    {
        [Key(1)]
        public string UserName { get; set; }
        [Key(2)]
        public string Password { get; set; }
    }
}
