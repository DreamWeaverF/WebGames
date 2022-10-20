using GameCommon;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeFightResultHander : AMessageNoticeHander<MessageNoticeFightResult>
    {
        [SerializeField]
        private UserStorage m_userStorage;
        protected override void OnMessage(MessageNoticeFightResult notice)
        {
            if (notice.WinUserId == m_userStorage.UserData.UserId)
            {
                m_userStorage.UserData.UserWinMatch += 1;
                m_userStorage.UserData.UserScore += 10;
            }
            else
            {
                m_userStorage.UserData.UserLoseMatch += 1;
                m_userStorage.UserData.UserScore -= 10;
            }
            m_userStorage.UserData.UserState = UserState.None;
        }
    }
}