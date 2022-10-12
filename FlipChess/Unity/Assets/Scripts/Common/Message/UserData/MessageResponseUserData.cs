using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    public class MessageResponseUserData : AMessageResponse
    {
        [Key(1)]
        public UserData UserData { get; set; }
        [Key(2)]
        public int FightID { get; set; }
    }
}
