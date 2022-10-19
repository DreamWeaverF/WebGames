using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestUserDataHander : AMessageRequestHander<MessageRequestUserData,MessageResponseUserData>
    {
        public override async Task<AMessageResponse> OnMessage(long userId, AMessageRequest request)
        {
            UserData userData = await m_userRespository.GetUserData(userId);
            m_response.UserData = userData;
            m_response.UserState = userData.UserState;
            m_response.FightID = userData.FightID;
            return m_response;
        }
    }
}