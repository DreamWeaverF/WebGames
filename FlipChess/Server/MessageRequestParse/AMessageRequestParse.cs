namespace Dreamwear
{
    public abstract class AMessageRequestParse<T1,T2> : AMessageRequestParse where T1 : AMessageRequest where T2 : AMessageResponse,new()
    {
        protected T2 m_response = new T2();

        public override AMessageResponse Parse(IMessage request)
        {
            ParseMessage(request as T1);
            return m_response;
        }

        protected abstract void ParseMessage(T1 request);

        public override Type GetRequestType()
        {
            return typeof(T1);
        }

        public override Type GetResponseType()
        {
            return typeof(T2);
        }
    }

    public abstract class AMessageRequestParse
    {
        public abstract AMessageResponse Parse(IMessage request);
        public abstract Type GetRequestType();
        public abstract Type GetResponseType();
    }
}
