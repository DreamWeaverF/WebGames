using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestJoinFightHander : AMessageRequestHander<MessageRequestJoinFight,MessageResponseJoinFight>
    {
        [SerializeField]
        private FightRespository m_fightResponsitory;
        protected override async Task OnMessage(UserData userData, MessageRequestJoinFight request)
        {
            await Task.CompletedTask;
            if(!m_fightResponsitory.FightDatas.TryGetValue(request.FightId,out FightData fightData))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if(fightData.RedCampUserId != userData.UserId && fightData.BlueCampUserId != userData.UserId)
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            userData.UserState = UserState.Fight;
            userData.FightID = request.FightId;
        }
    }
}