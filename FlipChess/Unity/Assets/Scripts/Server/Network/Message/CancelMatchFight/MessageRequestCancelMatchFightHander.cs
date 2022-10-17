using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [AutoGenSOClass]
    public class MessageRequestCancelMatchFightHander : AMessageRequestHander<MessageRequestCancelMatchFight, MessageResponseMatchFight>
    {
        [SerializeField]
        private UserRespository m_userRespository;
        protected override async Task OnMessage(long userId, MessageRequestCancelMatchFight request)
        {
            await Task.CompletedTask;
            if (!m_userRespository.UserDatas.TryGetValue(userId, out UserData userData))
            {
                m_response.ErrorCode = MessageErrorCode.UserNotLogged;
                return;
            }
            if (userData.UserState != UserState.Matching)
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            userData.UserState = UserState.None;
            SyncName.CancelMatchFight.BroadcastSyncEvent(userId);
        }
    }
}
