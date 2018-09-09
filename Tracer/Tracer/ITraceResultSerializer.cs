using System.IO;

namespace Tracer
{
    interface ITraceResultSerializer
    {
        Stream Stream
        { get; set; }

        void SerializeTraceResult(TraceResult traceResult);
    }
}
