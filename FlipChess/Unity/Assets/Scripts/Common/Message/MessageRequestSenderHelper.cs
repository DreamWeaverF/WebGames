using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    public static class MessageRequestSenderHelper
    {
        public static async ETTask<AMessageResponse> SendMessage<T1>(this T1 request) where T1 : AMessageRequest
        {
            ETTask<AMessageResponse> task = Enum_SyncName.MessageRequestSender.BroadcastSyncEvent<AMessageRequest,ETTask<AMessageResponse>>(request);
            return await task;
        }
    }
}
