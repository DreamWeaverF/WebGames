using GameCommon;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [AutoGenSOClass]
    public class MessageRequestUserDataHander : AMessageRequestHander<MessageRequestUserData,MessageResponseUserData>
    {
        [SerializeField]
        private UserRespository m_userRespository;
        protected override async Task OnMessage(long userId, MessageRequestUserData request)
        {
            UserData userData = await m_userRespository.GetUserData(userId);
            //todolist timer
            m_response.UserData = userData;
        }
    }
}
