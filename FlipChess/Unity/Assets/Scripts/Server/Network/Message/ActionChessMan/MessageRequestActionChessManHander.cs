
using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    public class MessageRequestActionChessManHander : AMessageRequestHander<MessageRequestActionChessMan, MessageResponseActionChessMan>
    {
        [SerializeField]
        private UserRespository m_userRespository;
        [SerializeField]
        private FightRespository m_fightRespository;
        [SerializeField]
        private MessageNoticeActionChessMan m_messageNotice;
        protected override async Task OnMessage(long userId, MessageRequestActionChessMan request)
        {
            await Task.CompletedTask;
            if (!m_userRespository.UserDatas.TryGetValue(userId, out UserData userData))
            {
                m_response.ErrorCode = MessageErrorCode.UserNotLogged;
                return;
            }
            if (userData.UserState != UserState.Fight)
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if(!m_fightRespository.FightDatas.TryGetValue(userData.FightID,out FightData fightData))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if(!fightData.CheckActionCheesMan(userId, request.CurPosition, request.TargetPosition))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            fightData.ExecuteActionChessMan(request.CurPosition, request.TargetPosition);
            m_messageNotice.CurPosition = request.CurPosition;
            m_messageNotice.TargetPosition = request.TargetPosition;
        }
    }
}
