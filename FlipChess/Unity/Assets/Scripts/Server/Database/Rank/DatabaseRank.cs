using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class DatabaseRankElement : ADatabaseElement
    {
        public int RankId;
        public RankData RankData;
    }
    [AutoGenSOClass]
    public class DatabaseRank : ADatabase<DatabaseRankElement,int>
    {

    }
}
