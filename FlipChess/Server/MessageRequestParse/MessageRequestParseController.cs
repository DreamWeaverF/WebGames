using Microsoft.AspNetCore.Mvc;

namespace Dreamwear
{
    public class MessageRequestParseController
    {
        private ILogger<MessageRequestParseController> m_logger;
        private Dictionary<Type, AMessageRequestParse> m_requestParses = new Dictionary<Type, AMessageRequestParse>();
        public MessageRequestParseController(ILogger<MessageRequestParseController> logger)
        {
            m_logger = logger;
            m_requestParses = new Dictionary<Type, AMessageRequestParse>();
            m_requestParses.Add(typeof(MessageRequestLogin),new MessageRequestLoginParse());
        }
        public IMessage ParseMessage(IMessage request,MessageUserData userData)
        {
            Type requestType = request.GetType();
            if(!m_requestParses.TryGetValue(requestType, out AMessageRequestParse requestParse))
            {
                m_logger.LogError($"找不到对应消息解析:{requestType.Name}");
                return null;
            }
            return requestParse.Parse(request);
        }
    }
}
