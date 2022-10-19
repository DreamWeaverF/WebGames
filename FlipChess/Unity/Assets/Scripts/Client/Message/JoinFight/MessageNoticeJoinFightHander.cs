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
        protected override void OnMessage(MessageNoticeJoinFight notice)
        {
            m_fightStorage.FightData.Users.Add(m_userStorage.UserData.UserId, new FightDataUser()
            {
                UserId = m_userStorage.UserData.UserId,
                UserNick = m_userStorage.UserData.UserNick,
                UserHeadIcon = m_userStorage.UserData.UserHeadIcon,
                EatChessMans = new List<int>()
            });
        }
    }
}