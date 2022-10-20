using Codice.CM.Common;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal;

namespace GameCommon
{
    [MessagePackObject]
    public class FightUserData
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
        public Dictionary<long, FightUserData> Users { get; set; }
        [Key(3)]
        public Dictionary<Vector2I, int> ChessMans { get; set; }    //0Î´·­¿ª -1¿Õ ID
        [Key(4)]
        public FightState State { get; set; }
        [Key(11)]
        public long LastTicks { get; set; }
        [Key(12)]
        public int RandomSeed { get; set; }
        [Key(13)]
        public int RandomCount { get; set; }
        [IgnoreMember]
        private readonly long m_waitIntervalTime = TimerStorage.SecondTick * 20;
        [IgnoreMember]
        private readonly long m_actionIntervalTime = TimerStorage.SecondTick * 20;
        [IgnoreMember]
        private Random m_random;
        [IgnoreMember]
        private long m_winUserId;
        [IgnoreMember]
        private FightResultType m_fightResultType;
        public void Update(TimerStorage timer)
        {
            switch (State)
            {
                case FightState.Start:
                    if(timer.Ticks - LastTicks < m_waitIntervalTime)
                    {
                        return;
                    }
                    State = FightState.End;
                    m_fightResultType = FightResultType.RunAway;
                    foreach(var fightUserData in Users.Values)
                    {
                        if(fightUserData.State == FightUserState.Start)
                        {
                            m_winUserId = fightUserData.UserId;
                        }
                    }
                    break;
                case FightState.Progress:
                    if (timer.Ticks - LastTicks < m_actionIntervalTime)
                    {
                        return;
                    }
                    m_fightResultType = FightResultType.RunAway;
                    foreach (var fightUserData in Users.Values)
                    {
                        if (fightUserData.State == FightUserState.WaitAction)
                        {
                            m_winUserId = fightUserData.UserId;
                        }
                    }
                    break;
            }
        }
        public void Reset(TimerStorage timer)
        {
            Users.Clear();
            ChessMans.Clear();
            State = FightState.Start;
            LastTicks = timer.Ticks;
            RandomSeed = new Random().Next(0, 10000);
            m_random = new Random(RandomSeed);
            RandomCount = 0;
        }
        public List<long> UserIds
        {
            get
            {
                return Users.Keys.ToArray().ToList();
            }
        }
        public long WinUserId
        {
            get
            {
                return m_winUserId;
            }
        }
        public FightResultType FightResultType
        {
            get
            {
                return m_fightResultType;
            }
        }
        public FightUserData InActionFightUserData
        {
            get
            {
                foreach (var value in Users.Values)
                {
                    if (value.State == FightUserState.InAction)
                    {
                        return value;
                    }
                }
                return null;
            }
        }
        public FightUserData WaitActionFightUserData
        {
            get
            {
                foreach (var value in Users.Values)
                {
                    if (value.State == FightUserState.WaitAction)
                    {
                        return value;
                    }
                }
                return null;
            }
        }
        private void AlternateRound(TimerStorage timerStorage)
        {
            foreach (var value in Users.Values)
            {
                value.State = value.State == FightUserState.WaitAction ? FightUserState.InAction : FightUserState.WaitAction;
            }
            LastTicks = timerStorage.Ticks;
        }
        public bool BindFightUserData(long userId, string userNick, string userHeadIcon, TimerStorage timer)
        {
            if (!Users.TryGetValue(userId, out FightUserData fightUser))
            {
                return false;
            }
            if(fightUser.State == FightUserState.Start)
            {
                return false;
            }
            fightUser.UserId = userId;
            fightUser.UserNick = userNick;
            fightUser.UserHeadIcon = userHeadIcon;
            fightUser.State = FightUserState.Start;
            fightUser.EatChessMans = new List<int>();
            bool isAction = true;
            foreach(FightUserData user in Users.Values)
            {
                if(user.State != FightUserState.Start)
                {
                    isAction = false;
                }
            }
            if (isAction)
            {
                State = FightState.Progress;
                for(int i = 1; i < 5; i++)
                {
                    for(int j = 1; j < 9; j++)
                    {
                        ChessMans.Add(new Vector2I() { x = i,y = j },0);
                    }
                }
                for(int i = 0; i < 4; i++)
                {
                    int random = RandomNumber % 32;
                    Vector2I pos = new Vector2I();
                    pos.x = random % 4 + 1;
                    pos.y = random / 4 + 1;
                    int id = RandomNumber % 32 + 1;
                    ChessMans[pos] = id;
                }
                FightUserData firstUser = Users.Values.First();
                firstUser.State = FightUserState.InAction;
                fightUser.State = FightUserState.WaitAction;
                LastTicks = timer.Ticks;
            }
            return true;
        }
        public bool CheckFlipChessMan(long userId, Vector2I targetPosition)
        {
            if(!Users.TryGetValue(userId,out FightUserData userData))
            {
                return false;
            }
            if(userData.State != FightUserState.InAction)
            {
                return false;
            }
            if(!ChessMans.TryGetValue(targetPosition,out int id))
            {
                return false;
            }
            if(id != 0)
            {
                return false;
            }
            return true;
        }
        public void ExecuteFlipCheesMan(Vector2I targetPosition,TimerStorage timerStorage,ConfigChessMan configChessMan)
        {
            ChessMans[targetPosition] = RandomNumber % 32 + 1;
            if(InActionFightUserData.Camp == FightCamp.None)
            {
                if(!configChessMan.TryGetValue(ChessMans[targetPosition], out ConfigChessManElement element))
                {
                    return;
                }
                InActionFightUserData.Camp = element.Camp;
                WaitActionFightUserData.Camp = element.Camp == FightCamp.Red ? FightCamp.Blue : FightCamp.Red;
            }
            AlternateRound(timerStorage);
        }
        public bool CheckActionCheesMan(long userId, Vector2I curPosition, Vector2I targetPosition, ConfigChessMan configChessMan)
        {
            if (!Users.TryGetValue(userId, out FightUserData userData))
            {
                return false;
            }
            if (userData.State != FightUserState.InAction)
            {
                return false;
            }
            if (!ChessMans.TryGetValue(curPosition, out int curId))
            {
                return false;
            }
            if(curId <= 0)
            {
                return false;
            }
            if(!configChessMan.TryGetValue(curId, out ConfigChessManElement element))
            {
                return false;
            }
            if (!ChessMans.TryGetValue(targetPosition, out int targetId))
            {
                return false;
            }
            switch (element.ActionType)
            {
                case ChessManActionType.Move:
                    if(targetId == 0)
                    {
                        return false;
                    }
                    int distance = Vector2I.Distance(curPosition, targetPosition);
                    if(distance != 1)
                    {
                        return false;
                    }
                    break;
                case ChessManActionType.Jump:
                    if(targetId == -1)
                    {
                        return false;
                    }
                    if(curPosition.x == targetPosition.x)
                    {
                        if(Math.Abs(targetPosition.y - curPosition.y) < 2)
                        {
                            return false;
                        }
                    }
                    if (curPosition.y == targetPosition.y)
                    {
                        if (Math.Abs(targetPosition.x - curPosition.x) < 2)
                        {
                            return false;
                        }
                    }
                    break;
            }
            if (targetId > 0)
            {
                if (!element.EatChessMans.Contains(targetId))
                {
                    return false;
                }
            }
            return true;
        }
        public void ExecuteActionChessMan(Vector2I curPosition, Vector2I targetPosition, TimerStorage timerStorage, ConfigChessMan configChessMan)
        {
            int targetId = ChessMans[targetPosition];
            if (!configChessMan.TryGetValue(targetId, out ConfigChessManElement element))
            {
                return;
            }
            foreach(var user in Users.Values)
            {
                if(user.Camp != element.Camp)
                {
                    user.EatChessMans.Add(targetId);
                    break;
                }
            }
            ChessMans[targetPosition] = ChessMans[curPosition];
            AlternateRound(timerStorage);
        }
        public bool CheckAdmitDefeat(long userId)
        {
            return true;
        }
        public void ExecuteAdmitDefeat(long userId)
        {

        }
        public bool CheckFightChat(string context)
        {
            return true;
        }
        public void ExecuteFigtChat(long userId, string context)
        {

        }
        public int RandomNumber
        {
            get
            {
                RandomCount++;
                int number = m_random.Next(0, 10000);
                return number;
            }
        }
    }
}
