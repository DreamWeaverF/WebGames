
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
        FetchMysqlConnection,
        RecycleMysqlConnection,
        //Client
        MessageRequestSender = 2000,
        LoadScene,
    }
}
