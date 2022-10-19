using System;
using System.Collections.Generic;

namespace GameCommon
{
    [Serializable]
    public class ConfigChessManElement : AConfigElement
    {
        public string Name;
        public string Pic;
        public FightCamp Camp;
        public ChessManActionType ActionType;
        public List<int> EatChessMans;
    }
    [GenerateAutoClass]
    public class ConfigChessMan : AConfig<ConfigChessManElement>
    {
        
    }
}
