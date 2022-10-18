using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestAdmitDefeatSender : AMessageRequestSender<MessageRequestAdmitDefeat,MessageResponseAdmitDefeat>
    {
        public async ETTask<MessageResponseAdmitDefeat> SendMessage(System.Int32 rpcId)
        {
			m_request.RpcId = rpcId;

            return await SendMessageCore();
        }
    }
}