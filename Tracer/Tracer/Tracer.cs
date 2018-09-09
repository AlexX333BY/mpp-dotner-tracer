using System;
using System.Threading;
using System.Reflection;

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
            MethodBase methodBase = MethodBase.GetCurrentMethod();
            MethodResult methodResult = new MethodResult();
            methodResult.ClassName = methodBase.ReflectedType.Name;
            methodResult.MethodName = methodBase.Name;
            ThreadResult curThreadResult = traceResult.AddOrGetThreadResult(Thread.CurrentThread.ManagedThreadId);
            curThreadResult.StartTracingMethod(methodResult);
        }

        public void StopTrace()
        {
            traceResult.GetThreadResult(Thread.CurrentThread.ManagedThreadId).StopTracingMethod();
        }

        public Tracer()
        {
            traceResult = new TraceResult();
        }
    }
}