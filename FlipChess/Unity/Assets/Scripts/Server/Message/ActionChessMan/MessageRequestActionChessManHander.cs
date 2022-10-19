using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestActionChessManHander : AMessageRequestHander<MessageRequestActionChessMan,MessageResponseActionChessMan>
    {
        [SerializeField]
        private FightRespository m_fightRespository;
        [SerializeField]
        private MessageNoticeActionChessManSender m_sender;
        protected override async Task OnMessage(UserData userData, MessageRequestActionChessMan request)
        {
            await Task.CompletedTask;
            if(!m_fightRespository.FightDatas.TryGetValue(userData.FightID,out FightData fightData))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if(!fightData.CheckActionCheesMan(userData.UserId, request.CurPosition, request.TargetPosition))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            fightData.ExecuteActionChessMan(request.CurPosition, request.TargetPosition);
            m_sender.SendMessage(fightData.UserIds, request.CurPosition, request.TargetPosition);
        }
    }
}