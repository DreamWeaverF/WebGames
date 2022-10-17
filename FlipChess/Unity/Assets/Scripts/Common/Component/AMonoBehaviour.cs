using UnityEngine;

namespace GameCommon
{
    public abstract class AMonoBehaviour : MonoBehaviour
    {
        void Start()
        {
            GetType().RegisterSyncClass(this);
            OnInit();
        }
        void OnDestroy()
        {
            GetType().UnRegisterSyncClass(this);
            UnInit();
        }
        protected abstract void OnInit();
        protected abstract void UnInit();
    }
}
