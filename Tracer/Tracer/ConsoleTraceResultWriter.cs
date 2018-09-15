using System;
using System.IO;

namespace Tracer
{
    public class ConsoleTraceResultWriter : ITraceResultWriter
    {
        public void Write(TraceResult traceResult, ITraceResultSerializer serializer)
        {
            using (Stream consoleOutputStream = Console.OpenStandardOutput())
            {
                serializer.SerializeTraceResult(traceResult, consoleOutputStream);
            }
        }
    }
}
