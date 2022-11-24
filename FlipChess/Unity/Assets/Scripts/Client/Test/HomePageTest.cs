using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    public class HomePageTest : AUI
    {
        [UIComponentField]
        [SerializeField]
        private FImage m_dyImage;

        protected override void OnInit()
        {
            m_dyImage.SetSprite("Assets/Arts/Sprites/TestSprite.png");
        }

    }
}
