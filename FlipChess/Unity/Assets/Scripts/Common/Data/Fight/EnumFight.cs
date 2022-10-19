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
        Wait,
        Action,
        End,
    }
    public enum FightUserState
    {
        None,
        Join,
        Wait,
        Action,
        End,
    }
}
