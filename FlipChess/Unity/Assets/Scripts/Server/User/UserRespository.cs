using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class UserRespository : ScriptableObject
    {
        [SerializeField]
        private DatabaseUser m_databaseUser;

        private Dictionary<long, UserData> m_userDatas = new Dictionary<long, UserData>();

    }
}
