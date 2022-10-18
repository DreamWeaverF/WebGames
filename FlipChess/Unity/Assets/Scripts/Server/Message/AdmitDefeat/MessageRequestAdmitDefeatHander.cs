using GameCommon;
using System.Threading.Tasks;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestAdmitDefeatHander : AMessageRequestHander<MessageRequestAdmitDefeat,MessageResponseAdmitDefeat>
    {
        protected override async Task OnMessage(UserData userData, MessageRequestAdmitDefeat request)
        {
            await Task.CompletedTask;
        }
    }
}