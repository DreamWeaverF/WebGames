using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    public class MessageResponseRankData : AMessageResponse
    {
        [Key(1)]
        public List<RankData> RankDatas;
    }
}
