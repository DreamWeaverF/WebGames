using GameCommon;
using System.Threading.Tasks;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestFightChatHander : AMessageRequestHander<MessageRequestFightChat,MessageResponseFightChat>
    {
        protected override async Task OnMessage(UserData userData, MessageRequestFightChat request)
        {
            await Task.CompletedTask;
        }
    }
}