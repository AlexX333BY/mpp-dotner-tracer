using System;

namespace Tracer
{
    public class Tracer : ITracer
    {
        protected TraceResult traceResult;

        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        public void StartTrace()
        {
            throw new NotImplementedException();
        }

        public void StopTrace()
        {
            throw new NotImplementedException();
        }

        public Tracer()
        {
            traceResult = new TraceResult();
        }
    }
}