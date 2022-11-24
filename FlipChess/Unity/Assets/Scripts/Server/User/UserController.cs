using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.CM.Common.CmCallContext;

namespace GameServer
{
    public class UserController : AMonoBehaviour
    {
        [SerializeField]
        private UserRespository m_userRespository;
        [SerializeField]
        private TimerStorage m_timerStorage;

        private readonly int m_checkClearIntervalMs = 60 * 1000;
        private readonly int m_checkMessageIntervalMs = 120 * 1000;

        private List<long> m_waitDelList = new List<long>();
        private long m_lastCheckMs = 0;
        protected override void OnInit()
        {
            m_lastCheckMs = m_timerStorage.MilliSecond;
        }
        protected override async void UnInit()
        {
            foreach (var var in m_userRespository.UserDatas)
            {
                await m_userRespository.DeleteUserData(var.Key);
            }
        }
        void Update()
        {
            CheckTimeOutUser();
        }
        private async void CheckTimeOutUser()
        {
            long currentMs = m_timerStorage.MilliSecond;
            if (currentMs - m_lastCheckMs < m_checkClearIntervalMs)
            {
                return;
            }
            m_waitDelList.Clear();
            foreach (var var in m_userRespository.UserDatas)
            {
                if (currentMs - var.Value.LastMessageTime < m_checkMessageIntervalMs)
                {
                    continue;
                }
                m_waitDelList.Add(var.Key);
            }
            for (int i = 0; i < m_waitDelList.Count; i++)
            {
                await m_userRespository.DeleteUserData(m_waitDelList[i]);
            }
            m_lastCheckMs = currentMs;
        }
    }
}
