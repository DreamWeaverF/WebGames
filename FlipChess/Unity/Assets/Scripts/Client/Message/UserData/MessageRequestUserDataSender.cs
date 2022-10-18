using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestUserDataSender : AMessageRequestSender<MessageRequestUserData,MessageResponseUserData>
    {
        public async ETTask<MessageResponseUserData> SendMessage()
        {

            return await SendMessageCore();
        }
    }
}