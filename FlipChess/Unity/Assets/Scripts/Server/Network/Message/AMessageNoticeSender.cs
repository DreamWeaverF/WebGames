using GameCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class AMessageNoticeSender<T1> : ScriptableObject
    {
        public virtual void SendMessage(T1 notice,List<long> userIds)
        {
            SyncName.MessageNoticeSender.BroadcastSyncEvent(notice, userIds);
        }
    }
}
