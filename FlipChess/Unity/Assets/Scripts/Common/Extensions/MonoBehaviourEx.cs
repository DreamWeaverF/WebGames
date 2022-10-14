using UnityEngine;

namespace GameCommon
{
    public class MonoBehaviourEx : MonoBehaviour
    {
        void Awake()
        {
            this.GetType().RegisterSyncClass(this);
        }
        void OnDestroy()
        {
            this.GetType().UnRegisterSyncClass(this);
            OnDestoryed();
        }

        protected virtual void OnDestoryed()
        {
            
        }
    }
}
