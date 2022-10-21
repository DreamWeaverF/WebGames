using Codice.CM.Common;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
        public bool IsEnter { get; set; }
    }
    [MessagePackObject]
    public class FightData
    {
        [Key(1)]
        public int FightId { get; set; }
        [Key(2)]
        public FightUserData UserData1 { get; set; }
        [Key(3)]
        public FightUserData UserData2 { get; set; }
        [Key(4)]
        public Dictionary<Vector2I, int> ChessMans { get; set; }    //0Î´·­¿ª -1¿Õ ID
        [Key(5)]
        public List<int> UnUseChessMans { get; set; }
        [Key(10)]
        public long ActionUserId { get; set; }
        [Key(11)]
        public FightState State { get; set; }
        [Key(12)]
        public long LastTicks { get; set; }
        [Key(13)]
        public int RandomSeed { get; set; }
        [Key(14)]
        public int RandomCount { get; set; }
        [Key(20)]
        public long WinUserId { get; set; }
        [Key(21)]
        public FightResultType ResultType { get; set; }

        [IgnoreMember]
        private readonly long m_waitIntervalTime = TimerStorage.SecondTick * 20;
        [IgnoreMember]
        private readonly long m_actionIntervalTime = TimerStorage.SecondTick * 20;
        [IgnoreMember]
        private Random m_random;

        public void Start(int fightId, long userId1, long userId2, TimerStorage timer)
        {
            State = FightState.Start;
            UserData1 = new FightUserData();
            UserData1.UserId = userId1;
            UserData2.UserId = userId2;
            UserData2 = new FightUserData();
            LastTicks = timer.Ticks;
            RandomSeed = new Random().Next(0, 10000);
            m_random = new Random(RandomSeed);
            RandomCount = 0;
            WinUserId = 0;
            ActionUserId = 0;
            ResultType = FightResultType.None;
            FightId = fightId;
        }
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
                    ResultType = FightResultType.NotEnter;
                    if (UserData1.IsEnter)
                    {
                        WinUserId = UserData1.UserId;
                    }
                    else
                    {
                        WinUserId = UserData2.UserId;
                    }
                    break;
                case FightState.Progress:
                    if (timer.Ticks - LastTicks < m_actionIntervalTime)
                    {
                        return;
                    }
                    State = FightState.End;
                    ResultType = FightResultType.RunAway;
                    if (UserData1.UserId != ActionUserId)
                    {
                        WinUserId = UserData1.UserId;
                    }
                    else
                    {
                        WinUserId = UserData2.UserId;
                    }
                    break;
            }
        }
        public List<long> UserIds
        {
            get
            {
                return new List<long>() { UserData1.UserId, UserData2.UserId };
            }
        }
        public long LoseUserId
        {
            get
            {
                if(UserData1.UserId == WinUserId)
                {
                    return UserData2.UserId;
                }
                return UserData1.UserId;
            }
        }
        private void AlternateRound(TimerStorage timerStorage)
        {
            ActionUserId = ActionUserId == UserData1.UserId ? UserData2.UserId : UserData1.UserId;
            LastTicks = timerStorage.Ticks;
        }
        public bool CheckEnterUser(long userId)
        {
            if(UserData1.UserId == userId)
            {
                if (UserData1.IsEnter)
                {
                    return false;
                }
                return true;
            }
            if (UserData2.UserId == userId)
            {
                if (UserData2.IsEnter)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public void ExecuteEnterUser(long userId, string userNick, string userHeadIcon, TimerStorage timer)
        {
            FightUserData enterUser = userId == UserData1.UserId ? UserData1 : UserData2;
            enterUser.UserId = userId;
            enterUser.UserNick = userNick;
            enterUser.UserHeadIcon = userHeadIcon;
            enterUser.EatChessMans = new List<int>();
            enterUser.IsEnter = true;
            if(!UserData1.IsEnter || !UserData2.IsEnter)
            {
                return;
            }
            State = FightState.Progress;
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    ChessMans.Add(new Vector2I() { x = i, y = j }, 0);
                }
            }
            for(int i = 1; i < 33; i++)
            {
                UnUseChessMans.Add(i);
            }
            for (int i = 0; i < 4; i++)
            {
                int random = RandomNumber % 32;
                Vector2I pos = new Vector2I();
                pos.x = random % 4 + 1;
                pos.y = random / 4 + 1;
                int id = RandomChessManId;
                ChessMans[pos] = id;
            }
            ActionUserId = UserData1.UserId;
            LastTicks = timer.Ticks;
        }
        public bool CheckFlipChessMan(long userId, Vector2I targetPosition)
        {
            if(userId != ActionUserId)
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
            ChessMans[targetPosition] = RandomChessManId;
            if(ActionUserId == UserData1.UserId && UserData1.Camp == FightCamp.None)
            {
                if (!configChessMan.TryGetValue(ChessMans[targetPosition], out ConfigChessManElement element))
                {
                    return;
                }
                UserData1.Camp = element.Camp;
                UserData2.Camp = element.Camp == FightCamp.Red ? FightCamp.Blue : FightCamp.Red;
            }
            else if(ActionUserId == UserData2.UserId && UserData2.Camp == FightCamp.None)
            {
                if (!configChessMan.TryGetValue(ChessMans[targetPosition], out ConfigChessManElement element))
                {
                    return;
                }
                UserData2.Camp = element.Camp;
                UserData1.Camp = element.Camp == FightCamp.Red ? FightCamp.Blue : FightCamp.Red;
            }
            AlternateRound(timerStorage);
        }
        public bool CheckActionCheesMan(long userId, Vector2I curPosition, Vector2I targetPosition, ConfigChessMan configChessMan)
        {
            if (userId != ActionUserId)
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
            if(targetId != -1)
            {
                if(targetId == 0)
                {
                    targetId = RandomChessManId;
                }
                if (!configChessMan.TryGetValue(targetId, out ConfigChessManElement element))
                {
                    return;
                }
                if(UserData1.Camp != element.Camp)
                {
                    UserData1.EatChessMans.Add(targetId);
                    UserData1.Score += element.Score;
                }
                else
                {
                    UserData2.EatChessMans.Add(targetId);
                    UserData2.Score += element.Score;
                }
            }
            ChessMans[curPosition] = -1;
            ChessMans[targetPosition] = ChessMans[curPosition];
            AlternateRound(timerStorage);
        }
        public bool CheckAdmitDefeat()
        {
            if(State != FightState.Progress)
            {
                return false;
            }
            return true;
        }
        public void ExecuteAdmitDefeat(long userId)
        {
            ResultType = FightResultType.AdmitDefeat;
            WinUserId = userId == UserData1.UserId ? UserData2.UserId : UserData1.UserId;
            State = FightState.End;
        }
        public bool CheckFightChat(string context)
        {
            return true;
        }
        public void ExecuteFigtChat(long userId, string context)
        {

        }
        private int RandomNumber
        {
            get
            {
                RandomCount++;
                int number = m_random.Next(0, 10000);
                return number;
            }
        }
        private int RandomChessManId
        {
            get
            {
                int randomIndex = RandomNumber % UnUseChessMans.Count;
                int randomId = UnUseChessMans[randomIndex];
                return randomId;
            }
        }
    }
}
