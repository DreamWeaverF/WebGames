using GameCommon;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [AutoGenSOClass]
    public class MessageRequestLoginHander : AMessageRequestHander<MessageRequestLogin,MessageResponseLogin>
    {
        [SerializeField]
        private DatabaseAccount m_databaseAccount;
        [SerializeField]
        private DatabaseUser m_databaseUser;

        protected override async Task OnMessage(long userId, MessageRequestLogin request)
        {
            long count = await m_databaseAccount.TrySelectCount(request.UserName);
            DatabaseAccountElement element = new DatabaseAccountElement();
            if(count == 0)
            {
                element.UserName = request.UserName;
                element.Password = request.Password;
                element.UserId = 100;
                await m_databaseAccount.TryInsertInto(element);
            }
            else
            {
                await m_databaseAccount.TrySelect(element, request.UserName);
                if(element.Password != request.Password)
                {
                    m_response.ErrorCode = MessageErrorCode.PasswordError;
                    return;
                }
            }
            m_response.UserId = element.UserId;
        }
    }
}
