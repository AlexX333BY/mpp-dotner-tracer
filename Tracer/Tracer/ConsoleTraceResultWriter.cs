using System;

namespace Tracer
{
    public class ConsoleTraceResultWriter : ITraceResultWriter
    {
        public ITraceResultSerializer Serializer
        { set; protected get; }

        public void Write(TraceResult traceResult)
        {
            Serializer.Stream = Console.OpenStandardOutput();
            Serializer.SerializeTraceResult(traceResult);
        }

        public ConsoleTraceResultWriter()
        {
            Serializer = null;
        }

        public ConsoleTraceResultWriter(ITraceResultSerializer serializer)
        {
            Serializer = serializer;
        }
    }
}
