using MessagePack;

namespace GameCommon
{
    public class MessageNoticeError : AMessageNotice
    {
        [Key(1)]
        public MessageErrorCode ErrorCode { get; set; }
    }
}
