using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    public static class FightDataExtensions 
    {
        public static void Update(this FightData fightData)
        {
            //fightData.LastOpearTime;
        }
        public static bool CheckActionCheesMan(this FightData fightData, long userId, Vector2I curPosition, Vector2I targetPosition)
        {
            return false;
        }
        public static void ExecuteActionChessMan(this FightData fightData,Vector2I curPosition,Vector2I targetPosition)
        {

        }
        //
        public static bool CheckAdmitDefeat(this FightData fightData, long userId)
        {
            return true;
        }
        public static void ExecuteAdmitDefeat(this FightData fightData,long userId)
        {

        }
        //
        public static bool CheckChat(this FightData fightData,long userId,string context)
        {
            return true;
        }
        public static void ExecuteChat(this FightData fightData, string context)
        {

        }
        //
        public static bool CheckFlipChessMan(this FightData fightData,long userId,Vector2I targetPosition)
        {
            return true;
        }
        public static void ExecuteFilpCheesMan(this FightData fightData,Vector2I targetPosition)
        {

        }
    }
}
