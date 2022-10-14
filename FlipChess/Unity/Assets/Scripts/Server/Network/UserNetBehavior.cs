using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine;
using System;
using MessagePack;
using GameCommon;

namespace GameServer
{
    public class UserNetBehavior : WebSocketBehavior
    {
        public Action<long, MessageEventArgs, UserNetBehavior> OnActionMessage;
        public Action<long> OnActionClose;

        private long m_userId;

        public long UserId
        {
            get { return m_userId; }
            set { m_userId = value; }
        }
        protected override void OnOpen()
        {
            base.OnOpen();
            if (this.Context.IsWebSocketRequest)
            {
                return;
            }
            CloseAsync();
        }
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            OnActionClose(m_userId);
        }
        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            Debug.LogError($"Íæ¼ÒÍøÂ·´íÎó{m_userId}");
            CloseAsync();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            if (!this.Context.IsWebSocketRequest)
            {
                CloseAsync();
                return;
            }
            OnActionMessage(m_userId, e, this);
        }
        public void SendMessage(byte[] bytes, bool bClose = false)
        {
            if (!bClose)
            {
                SendAsync(bytes, null);
                return;
            }
            SendAsync(bytes, (isSend) =>
            {
                CloseAsync();
            });
        }
    }
}
