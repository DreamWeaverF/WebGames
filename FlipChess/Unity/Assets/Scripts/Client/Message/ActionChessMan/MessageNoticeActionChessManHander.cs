using GameCommon;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeActionChessManHander : AMessageNoticeHander<MessageNoticeActionChessMan>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        protected override void OnMessage(MessageNoticeActionChessMan notice)
        {
            m_fightStorage.FightData.ExecuteActionChessMan(notice.CurPosition, notice.TargetPosition);
        }
    }
}