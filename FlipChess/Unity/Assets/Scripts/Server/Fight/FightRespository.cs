using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class FightRespository : ScriptableObject
    {
        public Dictionary<int, FightData> FightDatas = new Dictionary<int, FightData>();
    }
}
