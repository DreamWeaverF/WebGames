using GameCommon;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeActionChessManHander : AMessageNoticeHander<MessageNoticeActionChessMan>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        [SerializeField]
        private TimerStorage m_timerStorage;
        [SerializeField]
        private ConfigChessMan m_configChessMan;
        protected override void OnMessage(MessageNoticeActionChessMan notice)
        {
            m_fightStorage.FightData.ExecuteActionChessMan(notice.CurPosition, notice.TargetPosition, m_timerStorage, m_configChessMan);
        }
    }
}