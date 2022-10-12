using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace Dreamwear
{
    public class UserController
    {
        private ILogger<UserController> m_logger;
        private MessageRequestParseController m_requestParse;

        private Dictionary<long, MessageUserData> m_userDatas = new Dictionary<long, MessageUserData>();
        public UserController(ILogger<UserController> logger, MessageRequestParseController requestParse)
        {
            m_logger = logger;
            m_requestParse = requestParse;
        }
        public void Connect(long userId,string socketId)
        {
            if (!m_userDatas.TryGetValue(userId, out MessageUserData userData))
            {
                //todolist 查表拿数据填入
            }
            userData.SocketId = socketId;
        }
        public void UserDisConnect(long userId,string socketId)
        {
            if (!m_userDatas.TryGetValue(userId, out MessageUserData userData))
            {
                return;
            }
            if(userData.SocketId != socketId)
            {
                return;
            }
            m_userDatas.Remove(userId);
        }
        public void UserReciveMessage(long userId, string socketId, IMessage request)
        {
            if (!m_userDatas.TryGetValue(userId,out MessageUserData userData))
            {
                return;
            }
            m_requestParse.ParseMessage(request, userData);
        }
    }
}
