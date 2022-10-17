using MessagePack;
using System.Collections.Generic;

namespace GameCommon
{
    [MessagePackObject]
    public class FightData
    {
        [Key(100)]
        public int FightId { get; set; }
        [Key(1)]
        public long RedCampUserId { get; set; }
        [Key(2)]
        public int RedCampScore { get; set; }
        [Key(3)]
        public string RedCampNick { get; set; }
        [Key(4)]
        public string RedCampHeadIcon { get; set; }
        [Key(5)]
        public List<int> RedCampEatList { get; set; }
        [Key(11)]
        public long BlueCampUserId { get; set; }
        [Key(12)]
        public int BlueCampScore { get; set; }
        [Key(13)]
        public string BlueCampNick { get; set; }
        [Key(14)]
        public string BlueCampHeadIcon { get; set; }
        [Key(15)]
        public List<int> BlueCampEatList { get; set; }
        [Key(15)]
        public long LastOpearTime { get; set; }
        [Key(16)]
        public FightCamp PlayCamp { get; set; }
        [Key(17)]
        public int RandomSeed { get; set; }
        [Key(18)]
        public int RandomCount { get; set; }
        [Key(19)]
        public bool IsAction { get; set; }

    }
}
