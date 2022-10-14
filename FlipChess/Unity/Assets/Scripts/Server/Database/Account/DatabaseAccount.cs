using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class DatabaseAccountElement : DatabaseBaseElement
    {
        public string UserName;
        public string Password;
        public long UserId;
        public long LastLoginTime;
    }
    [AutoGenSOClass]
    public class DatabaseAccount : DatabaseBase<DatabaseAccountElement,string>
    {
        
    }
}
