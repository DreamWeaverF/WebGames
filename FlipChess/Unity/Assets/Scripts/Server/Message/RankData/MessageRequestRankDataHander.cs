using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageRequestRankDataHander : AMessageRequestHander<MessageRequestRankData,MessageResponseRankData>
    {
        [SerializeField]
        private RankRespository m_rankRespository;
        protected override async Task OnMessage(UserData userData, MessageRequestRankData request)
        {
            await Task.CompletedTask;
            m_response.RankDataElements = m_rankRespository.RankDataElements;
        }
    }
}