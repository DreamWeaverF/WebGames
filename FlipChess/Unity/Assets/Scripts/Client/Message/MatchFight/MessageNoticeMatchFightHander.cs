using GameCommon;
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
        private TimerStorage m_timerStorage;
        protected override void OnMessage(MessageNoticeMatchFight notice)
        {
            m_userStorage.UserData.UserState = UserState.Fight;
            m_userStorage.UserData.FightID = notice.FightId;
            m_fightStorage.FightData.Start(notice.FightId,notice.UserId1,notice.UserId2,m_timerStorage);
        }
    }
}