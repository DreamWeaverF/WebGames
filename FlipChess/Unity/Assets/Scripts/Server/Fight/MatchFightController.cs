using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class MatchFightController : AMonoBehaviour
    {
        private Dictionary<long, int> m_matchUsers = new Dictionary<long, int>();
        protected override void OnInit()
        {

        }
        protected override void UnInit()
        {

        }
        [SynchronizeMethod(SyncName = SyncName.MatchFight)]
        private void OnMatchFight(long userId,int userScore)
        {
            m_matchUsers.Add(userId, userScore);
        }
        [SynchronizeMethod(SyncName = SyncName.CancelMatchFight)]
        private void OnCancelMatchFight(long userId)
        {
            if (!m_matchUsers.ContainsKey(userId))
            {
                Debug.LogError($"玩家不在匹配队列中:{userId}");
                return;
            }
            m_matchUsers.Remove(userId);
        }
        public void Update()
        {
            if (m_matchUsers.Count < 2)
            {
                return;
            }
            long startUserId = 0;
            int startUserScore = 0;
            long matchUserId = 0;
            int scoreAbs = int.MaxValue;
            foreach (var kv in m_matchUsers)
            {
                if (startUserId == 0)
                {
                    startUserId = kv.Key;
                    startUserScore = kv.Value;
                    continue;
                }
                if (Math.Abs(kv.Value - startUserScore) >= scoreAbs)
                {
                    continue;
                }
                scoreAbs = Math.Abs(kv.Value - startUserScore);
                matchUserId = kv.Key;
            }
            if (matchUserId <= 0)
            {
                return;
            }
            SyncName.MatchFightSuccess.BroadcastSyncEvent(startUserId,matchUserId);
        }
    }
}
