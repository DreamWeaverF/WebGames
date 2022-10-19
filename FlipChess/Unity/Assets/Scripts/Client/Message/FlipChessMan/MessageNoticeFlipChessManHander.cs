using GameCommon;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeFlipChessManHander : AMessageNoticeHander<MessageNoticeFlipChessMan>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        protected override void OnMessage(MessageNoticeFlipChessMan notice)
        {
            m_fightStorage.FightData.ExecuteFlipCheesMan(notice.TargetPosition);
        }
    }
}