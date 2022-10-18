using GameCommon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestLoginHander : AMessageRequestHander<MessageRequestLogin,MessageResponseLogin>
    {
        [SerializeField]
        private DatabaseAccount m_databaseAccount;

        private Dictionary<string, DatabaseAccountElement> m_accountElements = new Dictionary<string, DatabaseAccountElement>();
        private readonly int m_maxCacheCount = 100;

        private DatabaseAccountElement m_element;

        private long m_tempUserId;

        public override async Task<AMessageResponse> OnMessage(long userId, AMessageRequest request)
        {
            MessageRequestLogin requestLogin = request as MessageRequestLogin;
            if (!m_accountElements.TryGetValue(requestLogin.UserName, out DatabaseAccountElement element))
            {
                long count = await m_databaseAccount.TrySelectCount(requestLogin.UserName);
                if (count > 0)
                {
                    await m_databaseAccount.TrySelect(m_element, requestLogin.UserName);
                }
                else
                {
                    element = new DatabaseAccountElement();
                    element.UserName = requestLogin.UserName;
                    element.Password = requestLogin.Password;
                    element.UserId = ++m_tempUserId;
                    await m_databaseAccount.TryInsertInto(m_element);
                }
                m_accountElements.Add(requestLogin.UserName, element);
                if(m_accountElements.Count > m_maxCacheCount)
                {
                    string temp = m_accountElements.First().Key;
                    m_accountElements.Remove(temp);
                }
            }
            if (requestLogin.Password != element.Password)
            {
                m_response.ErrorCode = MessageErrorCode.PasswordError;
                return m_response;
            }
            m_response.UserId = userId;
            return m_response;
        }
    }
}