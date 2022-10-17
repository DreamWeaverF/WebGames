using GameCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class FightController : AMonoBehaviour
    {
        [SerializeField]
        private UserRespository m_userRespository = new UserRespository();
        [SerializeField]
        private FightRespository m_fightRespository = new FightRespository();
        [SerializeField]
        private MessageNoticeMatchFightSender m_noticeMatchFightSender = new MessageNoticeMatchFightSender();

        private List<FightData> m_recyleFight = new List<FightData>();
        private int m_useFightId;
        protected override void OnInit()
        {

        }
        protected override void UnInit()
        {

        }
        [SynchronizeMethod(SyncName = SyncName.MatchFightSuccess)]
        private void OnMatchFightSuccess(long redUserID,long blueUserId)
        {
            m_userRespository.UserDatas.TryGetValue(redUserID, out UserData redUserData);
            m_userRespository.UserDatas.TryGetValue(blueUserId, out UserData blueUserData);
            FightData fightData;
            if (m_recyleFight.Count != 0)
            {
                fightData = m_recyleFight[0];
            }
            else
            {
                fightData = new FightData();
                fightData.FightId = ++m_useFightId;
            }

            fightData.RedCampUserId = redUserID;
            fightData.RedCampScore = 0;
            fightData.RedCampNick = redUserData.UserNick;
            fightData.RedCampHeadIcon = redUserData.UserHeadIcon;
            fightData.RedCampEatList.Clear();

            fightData.BlueCampUserId = redUserID;
            fightData.BlueCampScore = 0;
            fightData.BlueCampNick = redUserData.UserNick;
            fightData.BlueCampHeadIcon = redUserData.UserHeadIcon;
            fightData.BlueCampEatList.Clear();

            fightData.LastOpearTime = 0;
            fightData.PlayCamp = FightCamp.None;
            fightData.RandomSeed = 100;
            fightData.RandomCount = 0;
            fightData.IsAction = true;
            m_fightRespository.FightDatas.Add(fightData.FightId, fightData);
            m_noticeMatchFightSender.SendMessage(new List<long>() { redUserID, blueUserId }, fightData.FightId);
        }
        void Update()
        {
            List<int> endlessFightList = new List<int>();
            foreach(FightData data in m_fightRespository.FightDatas.Values)
            {
                data.Update();
                if (data.IsAction)
                {
                    continue;
                }
                endlessFightList.Add(data.FightId);
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
