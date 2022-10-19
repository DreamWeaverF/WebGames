using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestAdmitDefeatHander : AMessageRequestHander<MessageRequestAdmitDefeat,MessageResponseAdmitDefeat>
    {
        [SerializeField]
        private FightRespository m_fightRespository;
        protected override async Task OnMessage(UserData userData, MessageRequestAdmitDefeat request)
        {
            await Task.CompletedTask;
            if (!m_fightRespository.FightDatas.TryGetValue(userData.FightID, out FightData fightData))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if (!fightData.CheckAdmitDefeat(userData.UserId))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            fightData.ExecuteAdmitDefeat(userData.UserId);
        }
    }
}