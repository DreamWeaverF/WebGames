using GameCommon;
using ET;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    public class AMessageRequestSender<T1,T2> : ScriptableObject where T1 : AMessageRequest where T2 : AMessageResponse
    {
        public virtual async Task<T2> SendMessage(T1 request)
        {
            ETTask<AMessageResponse> task = SyncName.MessageRequestSender.BroadcastSyncEvent<AMessageRequest, ETTask<AMessageResponse>>(request);
            return await task as T2;
        }
    }
}
