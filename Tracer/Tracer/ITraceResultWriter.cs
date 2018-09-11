namespace Tracer
{
    interface ITraceResultWriter
    {
        ITraceResultSerializer Serializer
        { set; }

        void Write();
    }
}
