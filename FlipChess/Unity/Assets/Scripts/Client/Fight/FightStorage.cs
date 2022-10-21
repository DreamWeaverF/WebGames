using GameCommon;
using UnityEngine;
using System.Collections.Generic;

namespace GameClient
{
    [GenerateAutoClass]
    public class FightStorage : ScriptableObject
    {
        private FightData m_fightData;
        public FightData FightData
        {
            get
            {
                if(m_fightData == null)
                {
                    m_fightData = new FightData();
                    m_fightData.UserData1 = new FightUserData();
                    m_fightData.UserData2 = new FightUserData();
                    m_fightData.ChessMans = new Dictionary<Vector2I, int>();
                    m_fightData.UnUseChessMans = new List<int>();
                }
                return m_fightData;
            }
        }
    }
}
