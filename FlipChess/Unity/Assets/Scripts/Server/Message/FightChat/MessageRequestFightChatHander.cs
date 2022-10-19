using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestFightChatHander : AMessageRequestHander<MessageRequestFightChat,MessageResponseFightChat>
    {
        [SerializeField]
        private FightRespository m_fightRespository;
        [SerializeField]
        private MessageNoticeFightChatSender m_sender;
        protected override async Task OnMessage(UserData userData, MessageRequestFightChat request)
        {
            await Task.CompletedTask;
            if (!m_fightRespository.FightDatas.TryGetValue(userData.FightID, out FightData fightData))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if (!fightData.CheckFightChat(request.Context))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            fightData.ExecuteFigtChat(userData.UserId, request.Context);
            m_sender.SendMessage(fightData.UserIds,request.Context, userData.UserId);
        }
    }
}