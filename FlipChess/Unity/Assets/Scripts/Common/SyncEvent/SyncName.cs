
namespace GameCommon
{
    public enum SyncName
    {
        None,
        //Server
        MatchFight = 1000,
        CancelMatchFight,
        MatchFightSuccess,
        MessageNoticeSender,
        //Client
        MessageRequestSender = 2000,
    }
}
