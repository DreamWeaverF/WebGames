using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class FightRespository : ScriptableObject
    {
        private Dictionary<int, FightData> m_fightDatas = new Dictionary<int, FightData>();
    }
}
