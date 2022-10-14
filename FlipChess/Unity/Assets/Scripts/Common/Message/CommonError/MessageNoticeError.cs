using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    public class MessageNoticeError : AMessageNotice
    {
        [Key(1)]
        public MessageErrorCode ErrorCode { get; set; }
    }
}
