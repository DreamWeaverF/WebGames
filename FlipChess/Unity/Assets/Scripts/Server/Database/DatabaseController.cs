using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class DatabaseController : MonoBehaviour
    {
        [SerializeField]
        private DatabaseConnectPool m_connectPool;

        [SerializeField]
        private DatabaseUser m_databaseUser;

        [SerializeField]
        private DatabaseAccount m_databaseAccount;

        async void Start()
        {
            m_connectPool.Start();
            m_databaseUser.Start();
            m_databaseAccount.Start();
            //del test
            long test = await m_databaseUser.TrySelectCount(1001);

            await m_databaseAccount.TryInsertInto(new DatabaseAccountElement()
            {
                UserName = "xxx",
                Password = "xx1",
                UserId = 1001,
                LastLoginTime = 1,
            });
        }
    }
}
