using GameCommon;
using System.Threading.Tasks;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestFlipChessManHander : AMessageRequestHander<MessageRequestFlipChessMan,MessageResponseFlipChessMan>
    {
        protected override async Task OnMessage(UserData userData, MessageRequestFlipChessMan request)
        {
            await Task.CompletedTask;
        }
    }
}