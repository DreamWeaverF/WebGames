using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestAdmitDefeatSender : AMessageRequestSender<MessageRequestAdmitDefeat,MessageResponseAdmitDefeat>
    {
        public async ETTask<MessageResponseAdmitDefeat> SendMessage()
        {

            return await SendMessageCore();
        }
    }
}