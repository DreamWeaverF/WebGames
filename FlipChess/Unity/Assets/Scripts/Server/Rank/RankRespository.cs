using GameCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class RankRespository : ScriptableObject
    {
        [SerializeField]
        private DatabaseRank m_databaseRank;

        private readonly int m_rankId = 100;
        private readonly int m_maxRankCount = 1000;

        private DatabaseRankElement m_databaseRankElement = new DatabaseRankElement();

        public async void Init()
        {
            long rankCount = await m_databaseRank.TrySelectCount(m_rankId);
            if (rankCount == 0)
            {
                m_databaseRankElement.RankId = m_rankId;
                m_databaseRankElement.RankData = new RankData();
                m_databaseRankElement.RankData.RankId = m_rankId;
                m_databaseRankElement.RankData.RankDataElements = new List<RankDataElement>();
                await m_databaseRank.TryInsertInto(m_databaseRankElement);
            }
            else
            {
                await m_databaseRank.TrySelect(m_databaseRankElement, m_rankId);
            }
        }
        public async Task UnInit()
        {
            await m_databaseRank.TryUpdate(m_databaseRankElement);
        }
        public List<RankDataElement> RankDataElements
        {
            get
            {
                return m_databaseRankElement.RankData.RankDataElements;
            }
        }

        public int RefreshRank(UserData userData)
        {
            RankDataElement rankElement;
            int insertIndex = m_databaseRankElement.RankData.RankDataElements.Count;
            for (int i = m_databaseRankElement.RankData.RankDataElements.Count - 1; i >= 0; i--)
            {
                rankElement = m_databaseRankElement.RankData.RankDataElements[i];
                if(rankElement.UserId == userData.UserId)
                {
                    m_databaseRankElement.RankData.RankDataElements.RemoveAt(i);
                }
                if(userData.UserScore > rankElement.UserScore)
                {
                    insertIndex--;
                }
            }
            m_databaseRankElement.RankData.RankDataElements.Insert(insertIndex, new RankDataElement()
            {
                UserId = userData.UserId,
                UserScore = userData.UserScore,
                UserNick = userData.UserNick,
                UserHeadIcon = userData.UserHeadIcon
            });
            if(m_databaseRankElement.RankData.RankDataElements.Count > m_maxRankCount)
            {
                m_databaseRankElement.RankData.RankDataElements.RemoveAt(m_databaseRankElement.RankData.RankDataElements.Count - 1);
            }
            return insertIndex + 1;
        }

        public int GetCurrentUserRank(UserData userData)
        {
            RankDataElement rankElement;
            for (int i = m_databaseRankElement.RankData.RankDataElements.Count - 1; i >= 0; i--)
            {
                rankElement = m_databaseRankElement.RankData.RankDataElements[i];
                if (rankElement.UserId == userData.UserId)
                {
                    return i + 1;
                }
            }
            return m_databaseRankElement.RankData.RankDataElements.Count + 1;
        }
    }
}
