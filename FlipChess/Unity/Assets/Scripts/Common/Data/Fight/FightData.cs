using MessagePack;
using System.Collections.Generic;
using System.Linq;

namespace GameCommon
{
    [MessagePackObject]
    public class FightDataUser
    {
        [Key(1)]
        public long UserId { get; set; }
        [Key(2)]
        public string UserNick { get; set; }
        [Key(3)]
        public string UserHeadIcon { get; set; }
        [Key(4)]
        public FightCamp Camp { get; set; }
        [Key(5)]
        public int Score { get; set; }
        [Key(6)]
        public List<int> EatChessMans { get; set; }
        [Key(7)]
        public FightUserState State { get; set; }
    }
    [MessagePackObject]
    public class FightData
    {
        [Key(1)]
        public int FightId { get; set; }
        [Key(2)]
        public Dictionary<long, FightDataUser> Users { get; set; }
        [Key(3)]
        public Dictionary<Vector2I, int> ChessMans { get; set; }
        public FightState State { get; set; }
        [Key(11)]
        public long LastActionTime { get; set; }
        [Key(12)]
        public int RandomSeed { get; set; }
        [Key(13)]
        public int RandomCount { get; set; }

        public void Update()
        {
            //fightData.LastOpearTime;
        }
        public void Reset()
        {
            Users.Clear();
            ChessMans.Clear();
            State = FightState.Wait;
            LastActionTime = 0;
            RandomSeed = 0;
            RandomCount = 0;
        }
        public List<long> UserIds
        {
            get
            {
                return Users.Keys.ToArray().ToList();
            }
        }
        public bool CheckActionCheesMan(long userId, Vector2I curPosition, Vector2I targetPosition)
        {
            return false;
        }
        public void ExecuteActionChessMan(Vector2I curPosition, Vector2I targetPosition)
        {

        }
        //
        public bool CheckAdmitDefeat(long userId)
        {
            return true;
        }
        public void ExecuteAdmitDefeat(long userId)
        {

        }
        //
        public bool CheckFightChat(string context)
        {
            return true;
        }
        public void ExecuteFigtChat(long userId, string context)
        {

        }
        //
        public bool CheckFlipChessMan(long userId, Vector2I targetPosition)
        {
            return true;
        }
        public void ExecuteFlipCheesMan(Vector2I targetPosition)
        {

        }
    }
}
