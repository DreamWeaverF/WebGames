using GameCommon;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    public class AMessageNoticeHander<T1> : AMessageNoticeHander where T1 : AMessageNotice
    {
        public override Type GetNoticeType()
        {
            return typeof(T1);
        }
        public override void OnMessage(AMessageNotice notice)
        {
            OnMessage(notice as T1);
        }
        protected virtual void OnMessage(T1 notice)
        {

        }

    }

    public abstract class AMessageNoticeHander : ScriptableObject
    {
        public abstract Type GetNoticeType();

        public abstract void OnMessage(AMessageNotice notice);
    }
}
