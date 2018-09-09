using System.IO;

namespace Tracer
{
    public interface ITraceResultSerializer
    {
        Stream Stream
        { set; }

        void SerializeTraceResult(TraceResult traceResult);
    }
}
