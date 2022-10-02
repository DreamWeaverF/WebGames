using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreamwear
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SynchronizeMethodAttribute : BaseAttribute
    {
        public string SyncName;
    }
}
