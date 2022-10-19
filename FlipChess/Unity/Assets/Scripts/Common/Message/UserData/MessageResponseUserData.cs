using MessagePack;

namespace GameCommon
{
    public class MessageResponseUserData : AMessageResponse
    {
        [Key(1)]
        public UserData UserData { get; set; }
        [Key(2)]
        public UserState UserState { get; set; }
        [Key(3)]
        public int FightID { get; set; }
    }
}
