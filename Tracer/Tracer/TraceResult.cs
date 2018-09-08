using System.Collections.Generic;

namespace Tracer
{
    public class ThreadResult
    {
        private List<MethodResult> threadMethods;

        public string ThreadID
        { get; internal set; }
        public string Time
        { get; internal set; }
        public List<MethodResult> InnerMethods
        { get => new List<MethodResult>(threadMethods); }

        internal void AddThreadMethod(MethodResult methodResult)
        {
            threadMethods.Add(methodResult);
        }

        internal ThreadResult()
        {
            threadMethods = new List<MethodResult>();
        }
    }

    public class MethodResult
    {
        private List<MethodResult> innerMethods;

        public string MethodName
        { get; internal set; }
        public string ClassName
        { get; internal set; }
        public string Time
        { get; internal set; }
        public List<MethodResult> InnerMethods
        { get => new List<MethodResult>(innerMethods); }

        internal void AddInnerMethod(MethodResult methodResult)
        {
            innerMethods.Add(methodResult);
        }

        internal MethodResult()
        {
            innerMethods = new List<MethodResult>();
        }
    }

    public class TraceResult
    {
        private List<ThreadResult> threadResults;
        private readonly object threadLock;

        public List<ThreadResult> ThreadResults
        { get => new List<ThreadResult>(threadResults); }

        internal void AddThreadResult(ThreadResult threadResult)
        {
            lock(threadLock)
            {
                threadResults.Add(threadResult);
            }
        }

        internal TraceResult()
        {
            threadLock = new object();
            threadResults = new List<ThreadResult>();
        }
    }
}