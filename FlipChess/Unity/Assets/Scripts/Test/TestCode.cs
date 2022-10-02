using System.Net.WebSockets;
using UnityEngine;

namespace Dreamwear
{
    public class TestCode : MonoBehaviourEx
    {
        [SynchronizeField(SyncName = "xxx")]
        private float m_curTime;

        [SynchronizeMethod(SyncName = "xxx")]
        private void SyncTest(float code)
        {
            Debug.Log($"Test1 {code}");
        }

        public void OnClickDisplay()
        {
            m_curTime = Time.time;
        }
    }
}
