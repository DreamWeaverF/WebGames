using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestRankDataSender : AMessageRequestSender<MessageRequestRankData,MessageResponseRankData>
    {
        [SerializeField]
        private RankStorage m_rankStorage;
        public async Task<bool> SendMessage()
        {
            if(m_rankStorage.LastRefreshTime > 100)
            {
                return true;
            }
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            m_rankStorage.LastRefreshTime = 1000;
            m_rankStorage.Elements = m_response.RankDataElements;
            return true;
        }
    }
}