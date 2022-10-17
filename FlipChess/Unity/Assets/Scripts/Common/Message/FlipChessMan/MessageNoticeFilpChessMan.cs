using MessagePack;

namespace GameCommon
{
    public class MessageNoticeFilpChessMan : AMessageNotice
    {
        [Key(1)]
        public Vector2I TargetPosition { get; set; }
    }
}
