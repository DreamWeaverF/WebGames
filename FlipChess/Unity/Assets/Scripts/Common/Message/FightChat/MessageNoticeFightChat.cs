
using MessagePack;

namespace GameCommon
{
    public class MessageNoticeFightChat : AMessageNotice
    {
        [Key(1)]
        public string Context { get; set; }
        [Key(2)]
        public long UserId { get; set; }
    }
}
