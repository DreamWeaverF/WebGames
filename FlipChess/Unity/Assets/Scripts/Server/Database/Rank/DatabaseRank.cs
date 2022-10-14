using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class DatabaseRankElement : ADatabaseElement
    {
        public int RankId;
        public RankListData RankData;
    }
    [AutoGenSOClass]
    public class DatabaseRank : ADatabase<DatabaseRankElement,int>
    {

    }
}
