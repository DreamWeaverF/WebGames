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
        private MessageNoticeMatchFightSender m_matchFightSender;
        [SerializeField]
        private MessageNoticeFightResultSender m_fightResultSender;
        [SerializeField]
        private TimerStorage m_timerStorage;

        private List<FightData> m_recyleFight = new List<FightData>();
        private int m_useFightId;
        protected override void OnInit()
        {

        }
        protected override void UnInit()
        {

        }
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
                fightData.Users = new Dictionary<long, FightUserData>();
                fightData.ChessMans = new Dictionary<Vector2I, int>();
            }
            fightData.Reset(m_timerStorage);
            fightData.Users.Add(userId1, new FightUserData());
            fightData.Users.Add(userId2, new FightUserData());
            m_fightRespository.FightDatas.Add(fightData.FightId, fightData);
            m_matchFightSender.SendMessage(new List<long>() { userId1, userId2 }, fightData.FightId);
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
                    m_fightResultSender.SendMessage(data.UserIds, data.WinUserId, data.FightResultType);
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
