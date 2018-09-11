namespace Tracer
{
    public interface ITraceResultWriter
    {
        ITraceResultSerializer Serializer
        { set; }

        void Write(TraceResult traceResult);
    }
}
