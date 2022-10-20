using GameCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeJoinFightHander : AMessageNoticeHander<MessageNoticeJoinFight>
    {
        [SerializeField]
        private UserStorage m_userStorage;
        [SerializeField]
        private FightStorage m_fightStorage;
        [SerializeField]
        private TimerStorage m_timerStorage;
        protected override void OnMessage(MessageNoticeJoinFight notice)
        {
            m_fightStorage.FightData.Users.Add(m_userStorage.UserData.UserId, new FightUserData());
            m_fightStorage.FightData.BindFightUserData(notice.UserId,notice.UserNick,notice.UserHeadIcon, m_timerStorage);
        }
    }
}