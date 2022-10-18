using MessagePack;

namespace GameCommon
{
    [MessagePackObject]
    public class UserData
    {
        [Key(1)]
        public long UserId { get; set; }
        [Key(2)]
        public string UserNick { get; set; }
        [Key(3)]
        public string UserHeadIcon { get; set; }
        [Key(4)]
        public int UserScore { get; set; }
        [Key(5)]
        public int UserWinMatch { get; set; }
        [Key(6)]
        public int UserLoseMatch { get; set; }

        [IgnoreMember]
        public UserState UserState { get; set; }
        [IgnoreMember]
        public int FightID { get; set; }
        [IgnoreMember]
        public long LastMessageTime { get; set; }
    }
}
