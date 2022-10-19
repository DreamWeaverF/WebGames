using GameCommon;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestJoinFightHander : AMessageRequestHander<MessageRequestJoinFight,MessageResponseJoinFight>
    {
        [SerializeField]
        private FightRespository m_fightResponsitory;
        [SerializeField]
        private MessageNoticeJoinFightSender m_sender;
        protected override async Task OnMessage(UserData userData, MessageRequestJoinFight request)
        {
            await Task.CompletedTask;
            if(!m_fightResponsitory.FightDatas.TryGetValue(request.FightId,out FightData fightData))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            if (!fightData.Users.TryGetValue(userData.UserId,out FightDataUser fightUser))
            {
                m_response.ErrorCode = MessageErrorCode.MessageError;
                return;
            }
            fightUser.UserId = userData.UserId;
            fightUser.UserNick = userData.UserNick;
            fightUser.UserHeadIcon = userData.UserHeadIcon;
            fightUser.State = FightUserState.Join;
            fightUser.EatChessMans = new List<int>();
            userData.UserState = UserState.Fight;
            userData.FightID = request.FightId;
            m_sender.SendMessage(fightData.UserIds, fightUser.UserId,fightUser.UserNick,fightUser.UserHeadIcon);
        }
    }
}