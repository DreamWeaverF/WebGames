using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestFlipChessManHander : AMessageRequestHander<MessageRequestFlipChessMan,MessageResponseFlipChessMan>
    {
        [SerializeField]
        private FightRespository m_fightRespository;
        [SerializeField]
        private MessageNoticeFlipChessManSender m_sender;
        protected override async Task OnMessage(UserData userData, MessageRequestFlipChessMan request)
        {
            await Task.CompletedTask;
            if (!m_fightRespository.FightDatas.TryGetValue(userData.FightID, out FightData fightData))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if (!fightData.CheckFlipChessMan(userData.UserId,request.TargetPosition))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            fightData.ExecuteFlipCheesMan(request.TargetPosition);
            m_sender.SendMessage(fightData.UserIds, request.TargetPosition);
        }
    }
}