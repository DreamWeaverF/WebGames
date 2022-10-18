using GameCommon;
using System.Threading.Tasks;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestActionChessManHander : AMessageRequestHander<MessageRequestActionChessMan,MessageResponseActionChessMan>
    {
        protected override async Task OnMessage(UserData userData, MessageRequestActionChessMan request)
        {
            await Task.CompletedTask;
        }
    }
}