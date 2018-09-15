namespace Tracer
{
    public interface ITraceResultWriter
    {
        void Write(TraceResult traceResult, ITraceResultSerializer serializer);
    }
}
