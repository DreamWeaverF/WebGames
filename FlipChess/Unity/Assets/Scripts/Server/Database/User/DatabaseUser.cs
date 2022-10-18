using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class DatabaseUserElement : ADatabaseElement
    {
        public long UserId;
        public UserData UserData;
    }
    [GenerateAutoClass]
    public class DatabaseUser : ADatabase<DatabaseUserElement, long>
    {
        
    }
}
