using GameCommon;
using UnityEngine;

namespace GameClient
{
    public enum UIType
    {
        None,
        Page,
        Windows,
        Tip,
        Mask,
    }

    public abstract class AUI : AMonoBehaviour
    {
        [SerializeField]
        private UIType m_type;
    }
}
