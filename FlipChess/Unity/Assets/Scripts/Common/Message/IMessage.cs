using MessagePack;

namespace GameCommon
{
    [Union(100, typeof(MessageRequestLogin))]
    [Union(101, typeof(MessageResponseLogin))]
    public interface IMessage 
    {

    }
}
