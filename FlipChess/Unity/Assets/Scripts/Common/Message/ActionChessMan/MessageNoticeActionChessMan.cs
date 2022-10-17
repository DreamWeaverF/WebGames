

using MessagePack;

namespace GameCommon
{
    public class MessageNoticeActionChessMan : AMessageNotice
    {
        [Key(1)]
        public Vector2I CurPosition { get; set; }
        [Key(2)]
        public Vector2I TargetPosition { get; set; }
    }
}
