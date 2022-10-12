using MessagePack;

namespace GameCommon
{
    [MessagePackObject]
    public class FightData
    {
        [Key(1)]
        public long RedCampUserId { get; set; }
        [Key(2)]
        public int RedCampScore { get; set; }
        [Key(3)]
        public string RedCampNick { get; set; }
        [Key(4)]
        public string RedCampHeadIcon { get; set; }
        [Key(11)]
        public long BlueCampUserId { get; set; }
        [Key(12)]
        public int BlueCampScore { get; set; }
        [Key(13)]
        public string BlueCampNick { get; set; }
        [Key(14)]
        public string BlueCampHeadIcon { get; set; }
        [Key(15)]
        public long LastOpearTime { get; set; }
        [Key(16)]
        public int RandomSeed { get; set; }
        [Key(17)]
        public long PlayUserId { get; set; }

    }
}
