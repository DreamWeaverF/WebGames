using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SynchronizeMethodAttribute : BaseAttribute
    {
        public Enum_SyncName SyncName;
    }
}
