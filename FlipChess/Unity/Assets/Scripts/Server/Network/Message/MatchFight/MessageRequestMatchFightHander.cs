using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [AutoGenSOClass]
    public class MessageRequestMatchFightHander : AMessageRequestHander<MessageRequestMatchFight,MessageResponseMatchFight>
    {
        [SerializeField]
        private UserRespository m_userRespository;
        protected override async Task OnMessage(long userId, MessageRequestMatchFight request)
        {
            await Task.CompletedTask;
            if(!m_userRespository.UserDatas.TryGetValue(userId, out UserData userData))
            {
                m_response.ErrorCode = MessageErrorCode.UserNotLogged;
                return;
            }
            if(userData.UserState != UserState.None)
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            userData.UserState = UserState.Matching;
            SyncName.MatchFight.BroadcastSyncEvent(userId, userData.UserScore);
        }
    }
}
