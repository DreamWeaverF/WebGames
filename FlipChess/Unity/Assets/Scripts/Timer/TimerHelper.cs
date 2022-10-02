using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreamwear
{
    public class TimerHelper : MonoBehaviour
    {
        private readonly string m_updateEventName = "Update";
        private readonly string m_fixedupdateEventName = "FixedUpdate";

        void Update()
        {
            m_updateEventName.BroadcastSyncEvent();
        }

        void FixedUpdate()
        {
            m_fixedupdateEventName.BroadcastSyncEvent();
        }
    }
}
