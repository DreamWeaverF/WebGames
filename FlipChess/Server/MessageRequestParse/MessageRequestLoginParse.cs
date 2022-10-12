namespace Dreamwear
{
    public class MessageRequestLoginParse : AMessageRequestParse<MessageRequestLogin,MessageResponseLogin>
    {
        protected override void ParseMessage(MessageRequestLogin request)
        {
            m_response.UserId = 9654;
        }
    }
}
