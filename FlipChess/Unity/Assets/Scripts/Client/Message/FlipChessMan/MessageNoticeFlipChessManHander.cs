using GameCommon;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeFlipChessManHander : AMessageNoticeHander<MessageNoticeFlipChessMan>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        [SerializeField]
        private TimerStorage m_timerStorage;
        [SerializeField]
        private ConfigChessMan m_configChessMan;
        protected override void OnMessage(MessageNoticeFlipChessMan notice)
        {
            m_fightStorage.FightData.ExecuteFlipCheesMan(notice.TargetPosition, m_timerStorage, m_configChessMan);
        }
    }
}