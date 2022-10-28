using Codice.CM.Client.Differences.Merge;
using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameClient
{
    public class UIPageLoading : AUI
    {
        [UIComponentField]
        [SerializeField]
        private Text m_loadingTips;
        [UIComponentField]
        [SerializeField]
        private Image m_loadingProgress;
        protected override void OnInit()
        {
        }
        protected override void UnInit()
        {
        }
    }
}
