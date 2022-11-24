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
        protected virtual void OnInit()
        {

        }
        protected virtual void UnInit()
        {

        }
    }
}
