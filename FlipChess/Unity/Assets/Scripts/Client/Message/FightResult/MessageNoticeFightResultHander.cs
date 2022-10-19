using GameCommon;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeFightResultHander : AMessageNoticeHander<MessageNoticeFightResult>
    {
        [SerializeField]
        private UserStorage m_userStorage;
        [SerializeField]
        private FightStorage m_fightStorage;
        protected override void OnMessage(MessageNoticeFightResult notice)
        {
            if(!m_fightStorage.FightData.Users.TryGetValue(m_userStorage.UserData.UserId,out FightDataUser fightUser))
            {
                return;
            }
            if (fightUser.Camp == notice.WinCamp)
            {
                m_userStorage.UserData.UserWinMatch += 1;
            }
            else
            {
                m_userStorage.UserData.UserLoseMatch += 1;
            }
            m_userStorage.UserData.UserState = UserState.None;
        }
    }
}