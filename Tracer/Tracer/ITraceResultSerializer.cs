using System.IO;

namespace Tracer
{
    public interface ITraceResultSerializer
    {
        void SerializeTraceResult(TraceResult traceResult, Stream stream);
    }
}
