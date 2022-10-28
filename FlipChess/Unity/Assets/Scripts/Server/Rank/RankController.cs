using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class RankController : AMonoBehaviour
    {
        [SerializeField]
        private RankRespository m_rankREspository;

        protected override void OnInit()
        {
            m_rankREspository.Init();
        }

        protected override async void UnInit()
        {
            await m_rankREspository.UnInit();
        }
    }
}
