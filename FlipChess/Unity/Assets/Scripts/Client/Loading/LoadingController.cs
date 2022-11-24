using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace GameClient
{
    public class LoadingController : AMonoBehaviour
    {
        protected override void OnInit()
        {
            SyncName.LoadScene.BroadcastSyncEvent("Main");
        }
    }
}
