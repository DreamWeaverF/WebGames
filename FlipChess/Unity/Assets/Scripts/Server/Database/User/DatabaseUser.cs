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
    [AutoGenSOClass]
    public class DatabaseUser : ADatabase<DatabaseUserElement, long>
    {
        
    }
}
