using System.IO;

namespace Tracer
{
    interface ITraceResultSerializer
    {
        void SetStream(Stream stream);

        void SerializeTraceResult(TraceResult traceResult);
    }
}
