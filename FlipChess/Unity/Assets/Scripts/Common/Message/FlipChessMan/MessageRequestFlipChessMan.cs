using MessagePack;

namespace GameCommon
{
    public class MessageRequestFlipChessMan : AMessageRequest
    {
        [Key(1)]
        public Vector2I TargetPosition { get; set; }
    }
}
