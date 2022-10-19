using GameCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeMatchFightHander : AMessageNoticeHander<MessageNoticeMatchFight>
    {
        [SerializeField]
        private UserStorage m_userStorage;
        [SerializeField]
        private FightStorage m_fightStorage;
        [SerializeField]
        private MessageRequestJoinFightSender m_sender;
        protected override void OnMessage(MessageNoticeMatchFight notice)
        {
            m_userStorage.UserData.UserState = UserState.Fight;
            m_userStorage.UserData.FightID = notice.FightId;
            m_fightStorage.FightData.Reset();
            m_fightStorage.FightData.FightId = notice.FightId;
            m_sender.SendMessage(notice.FightId);
        }
    }
}