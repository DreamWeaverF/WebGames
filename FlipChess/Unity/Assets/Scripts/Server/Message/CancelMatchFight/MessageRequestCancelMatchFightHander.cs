using GameCommon;
using System.Threading.Tasks;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestCancelMatchFightHander : AMessageRequestHander<MessageRequestCancelMatchFight,MessageResponseCancelMatchFight>
    {
        protected override async Task OnMessage(UserData userData, MessageRequestCancelMatchFight request)
        {
            await Task.CompletedTask;
            if (userData.UserState != UserState.Matching)
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            SyncName.CancelMatchFight.BroadcastSyncEvent(userData.UserId);
            userData.UserState = UserState.None;
        }
    }
}