using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    public class MessageNoticeJoinFight : AMessageNotice
    {
        [Key(1)]
        public long UserId { get; set; }
        [Key(2)]
        public string UserNick { get; set; }
        [Key(3)]
        public string UserHeadIcon { get; set; }

    }
}
