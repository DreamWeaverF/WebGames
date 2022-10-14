using GameCommon;
using System.Threading.Tasks;

namespace GameServer
{
    [AutoGenSOClass]
    public class MessageRequestLoginHander : AMessageRequestHander<MessageRequestLogin,MessageResponseLogin>
    {
        public override async Task<AMessageResponse> OnMessage(long userId, AMessageRequest request)
        {
            await Task.CompletedTask;
            m_response.RpcId = request.RpcId;
            if (userId != 0)
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return m_response;
            }
            m_response.UserId = 10001;
            return m_response;
        }
    }
}
