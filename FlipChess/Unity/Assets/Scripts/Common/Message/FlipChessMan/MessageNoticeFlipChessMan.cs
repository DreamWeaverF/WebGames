using MessagePack;

namespace GameCommon
{
    public class MessageNoticeFlipChessMan : AMessageNotice
    {
        [Key(1)]
        public Vector2I TargetPosition { get; set; }
    }
}
