using GameCommon;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    [GenerateAutoClass]
    public class UserRespository : ScriptableObject
    {
        [SerializeField]
        private DatabaseUser m_databaseUser;

        private DatabaseUserElement m_databaseUserElement = new DatabaseUserElement();
        public Dictionary<long, UserData> UserDatas = new Dictionary<long, UserData>();
        public async Task<UserData> RequestUserData(long userId)
        {
            if(!UserDatas.TryGetValue(userId,out UserData userData))
            {
                long count = await m_databaseUser.TrySelectCount(userId);
                if (count == 0)
                {
                    userData = new UserData();
                    userData.UserId = userId;
                    userData.UserNick = "xxx";
                    userData.UserHeadIcon = "https://internal-public.oss-cn-beijing.aliyuncs.com/uc/84e3406b4ed249a98e1d47f95fd0dd0e.png";
                    m_databaseUserElement.UserId = userId;
                    m_databaseUserElement.UserData = userData;
                    await m_databaseUser.TryInsertInto(m_databaseUserElement);
                }
                else
                {
                    await m_databaseUser.TrySelect(m_databaseUserElement, userId);
                }
                UserDatas.Add(userId, userData);
            }
            return userData;
        }
        public async Task DeleteUserData(long userId)
        {
            if (!UserDatas.TryGetValue(userId, out UserData userData))
            {
                return;
            }
            m_databaseUserElement.UserId = userId;
            m_databaseUserElement.UserData = userData;
            await m_databaseUser.TryUpdate(m_databaseUserElement);
        }
    }
}
