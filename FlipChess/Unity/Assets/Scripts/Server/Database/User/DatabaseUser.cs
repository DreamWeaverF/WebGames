using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class DatabaseUserElement : DatabaseBaseElement
    {
        public long UserId;
        public UserData UserData;
    }
    [AutoGenSOClass]
    public class DatabaseUser : DatabaseBase<DatabaseBaseElement,long>
    {
        
    }
}
