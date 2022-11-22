namespace GameCommon
{
    public enum MessageErrorCode
    {
        Success,

        MessageError,           //消息错误
        RepeatLogin,            //重复登陆
        OtherLogin,             //其他地方登录
        ServerClose,            //服务器已关闭
        UserNotLogged,          //用户未登录
        PasswordError,          //密码错误
    }
}
