using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

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
            MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
            MethodResult methodResult = new MethodResult();
            methodResult.ClassName = methodBase.ReflectedType.Name;
            methodResult.MethodName = methodBase.Name;
            ThreadResult curThreadResult = traceResult.AddOrGetThreadResult(Thread.CurrentThread.ManagedThreadId);
            curThreadResult.StartTracingMethod(methodResult);
        }

        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                traceResult.GetThreadResult(threadId).StopTracingMethod();
            }
            catch (KeyNotFoundException e)
            {
                throw new NoMethodTracingException(threadId);
            }
        }

        public Tracer()
        {
            traceResult = new TraceResult();
        }
    }
}
