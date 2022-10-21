using GameCommon;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestJoinFightHander : AMessageRequestHander<MessageRequestJoinFight,MessageResponseJoinFight>
    {
        [SerializeField]
        private FightRespository m_fightResponsitory;
        [SerializeField]
        private MessageNoticeJoinFightSender m_sender;
        [SerializeField]
        private TimerStorage m_timerStorage;
        protected override async Task OnMessage(UserData userData, MessageRequestJoinFight request)
        {
            await Task.CompletedTask;
            if(!m_fightResponsitory.FightDatas.TryGetValue(request.FightId,out FightData fightData))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if (!fightData.CheckEnterUser(userData.UserId))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            fightData.ExecuteEnterUser(userData.UserId, userData.UserNick, userData.UserHeadIcon, m_timerStorage);
            m_sender.SendMessage(fightData.UserIds, userData.UserId, userData.UserNick, userData.UserHeadIcon);
        }
    }
}