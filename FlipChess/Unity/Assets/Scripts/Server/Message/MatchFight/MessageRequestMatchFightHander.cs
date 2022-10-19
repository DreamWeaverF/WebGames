using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestMatchFightHander : AMessageRequestHander<MessageRequestMatchFight,MessageResponseMatchFight>
    {
        protected override async Task OnMessage(UserData userData, MessageRequestMatchFight request)
        {
            await Task.CompletedTask;
            if(userData.UserState != UserState.None)
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            SyncName.MatchFight.BroadcastSyncEvent(userData.UserId);
            userData.UserState = UserState.Matching;
        }
    }
}