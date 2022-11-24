using GameCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class FightController : AMonoBehaviour
    {
        [SerializeField]
        private FightRespository m_fightRespository;
        [SerializeField]
        private UserRespository m_userRespository;
        [SerializeField]
        private MessageNoticeMatchFightSender m_matchFightSender;
        [SerializeField]
        private MessageNoticeFightResultSender m_fightResultSender;
        [SerializeField]
        private TimerStorage m_timerStorage;

        private List<FightData> m_recyleFight = new List<FightData>();
        private int m_useFightId;
        [SynchronizeMethod(SyncName = SyncName.MatchFightSuccess)]
        private void OnMatchFightSuccess(long userId1,long userId2)
        {
            FightData fightData;
            if (m_recyleFight.Count != 0)
            {
                fightData = m_recyleFight[0];
                m_recyleFight.RemoveAt(0);
            }
            else
            {
                fightData = new FightData();
                fightData.FightId = ++m_useFightId;
                fightData.ChessMans = new Dictionary<Vector2I, int>();
                fightData.UnUseChessMans = new List<int>();
            }
            fightData.Start(fightData.FightId,userId1,userId2, m_timerStorage);
            m_fightRespository.FightDatas.Add(fightData.FightId, fightData);
            m_matchFightSender.SendMessage(new List<long>() { userId1, userId2 }, fightData.FightId,userId1,userId2);
        }
        void Update()
        {
            List<int> endlessFightList = new List<int>();
            foreach(FightData data in m_fightRespository.FightDatas.Values)
            {
                data.Update(m_timerStorage);
                if (data.State == FightState.End)
                {
                    endlessFightList.Add(data.FightId);
                    //
                    m_fightResultSender.SendMessage(data.UserIds, data.WinUserId, data.ResultType);
                    if(m_userRespository.UserDatas.TryGetValue(data.WinUserId,out UserData winUserData))
                    {
                        winUserData.FightWin();
                    }
                    if(m_userRespository.UserDatas.TryGetValue(data.LoseUserId,out UserData loseUserData))
                    {
                        loseUserData.FightLose();
                    }
                }
            }
            if(endlessFightList.Count <= 0)
            {
                return;
            }
            for(int i = 0; i < endlessFightList.Count; i++)
            {
                m_recyleFight.Add(m_fightRespository.FightDatas[endlessFightList[i]]);
                m_fightRespository.FightDatas.Remove(endlessFightList[i]);
            }
        }
    }
}
