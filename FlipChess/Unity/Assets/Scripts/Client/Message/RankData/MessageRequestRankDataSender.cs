using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestRankDataSender : AMessageRequestSender<MessageRequestRankData,MessageResponseRankData>
    {
        public async ETTask<MessageResponseRankData> SendMessage(System.Int32 rankId)
        {
			m_request.RankId = rankId;

            return await SendMessageCore();
        }
    }
}