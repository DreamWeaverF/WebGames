using GameCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class FightController : AMonoBehaviour
    {
        [SerializeField]
        private FightRespository m_fightRespository = new FightRespository();
        [SerializeField]
        private MessageNoticeMatchFightSender m_sender = new MessageNoticeMatchFightSender();

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
                fightData.Users = new Dictionary<long, FightDataUser>();
                fightData.ChessMans = new Dictionary<Vector2I, int>();
            }
            fightData.Reset();
            fightData.Users.Add(userId1, new FightDataUser());
            fightData.Users.Add(userId2, new FightDataUser());
            m_fightRespository.FightDatas.Add(fightData.FightId, fightData);
            m_sender.SendMessage(new List<long>() { userId1, userId2 }, fightData.FightId);
        }
        void Update()
        {
            List<int> endlessFightList = new List<int>();
            foreach(FightData data in m_fightRespository.FightDatas.Values)
            {
                data.Update();
                if (data.State == FightState.End)
                {
                    endlessFightList.Add(data.FightId);
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
