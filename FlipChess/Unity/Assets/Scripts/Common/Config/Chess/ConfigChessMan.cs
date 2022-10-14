using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    [System.Serializable]
    public class ConfigChessManElement : AConfigElement
    {
        public string Name;
        public string Pic;
        public List<int> EatChessMans;
    }

    public class ConfigChessMan : AConfig<ConfigChessManElement>
    {
        
    }
}
