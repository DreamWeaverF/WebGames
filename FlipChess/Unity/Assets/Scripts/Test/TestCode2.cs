using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreamwear
{
    public class TestCode2 : MonoBehaviourEx
    {
        [SynchronizeMethod(SyncName = "xxx")]
        private void SyncTest2(float code)
        {
            Debug.Log($"Test2 {code}");
        }

        [SynchronizeMethod(SyncName = "xxx")]
        public void SyncTest3(float code)
        {
            Debug.Log($"Test3 {code}");
        }
    }
}
