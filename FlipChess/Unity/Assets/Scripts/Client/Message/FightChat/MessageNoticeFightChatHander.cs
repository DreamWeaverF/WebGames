using GameCommon;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageNoticeFightChatHander : AMessageNoticeHander<MessageNoticeFightChat>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        protected override void OnMessage(MessageNoticeFightChat notice)
        {
            m_fightStorage.FightData.ExecuteFigtChat(notice.UserId,notice.Context);
        }
    }
}