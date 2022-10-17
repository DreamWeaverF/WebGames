using MessagePack;

namespace GameCommon
{
    public class MessageResponseUserData : AMessageResponse
    {
        [Key(1)]
        public UserData UserData { get; set; }
    }
}
