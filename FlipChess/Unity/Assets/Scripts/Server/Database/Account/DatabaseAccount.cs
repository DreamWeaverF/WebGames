using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class DatabaseAccountElement : ADatabaseElement
    {
        public string UserName;
        public string Password;
        public long UserId;
        public long LastLoginTime;
    }
    [AutoGenSOClass]
    public class DatabaseAccount : ADatabase<DatabaseAccountElement,string>
    {
        
    }
}
