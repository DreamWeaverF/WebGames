using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreamwear
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SynchronizeFieldAttribute : BaseAttribute
    {
        public string SyncName;
    }
}
