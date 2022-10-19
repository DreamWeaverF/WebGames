using GameCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class RankStorage : ScriptableObject
    {
        public List<RankDataElement> Elements;

        public long LastRefreshTime;
    }
}
