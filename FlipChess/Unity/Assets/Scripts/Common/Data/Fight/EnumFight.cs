using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    public enum FightResultType
    {
        None,
        Normal,
        AdmitDefeat,
        NotEnter,
        RunAway,
    }
    public enum FightCamp
    {
        None,
        Red,
        Blue,
    }
    public enum ChessManActionType
    {
        None,
        Move,
        Jump,
    }
    public enum FightState
    {
        None,
        Start,
        Progress,
        End,
    }
}
