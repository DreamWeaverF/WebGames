using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestFightChatSender : AMessageRequestSender<MessageRequestFightChat,MessageResponseFightChat>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        public async Task<bool> SendMessage(System.String context)
        {
            if (m_fightStorage.FightData.CheckFightChat(context))
            {
                return false;
            }
			m_request.Context = context;
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            
            return true;
        }
    }
}