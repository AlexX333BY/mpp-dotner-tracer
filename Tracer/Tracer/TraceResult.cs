using System.Collections.Generic;

namespace Tracer
{
    public class ThreadResult
    {
        private List<MethodResult> threadMethods;

        public int ThreadID
        { get; internal set; }
        public string Time
        { get; internal set; }
        public List<MethodResult> InnerMethods
        { get => new List<MethodResult>(threadMethods); }

        internal void AddThreadMethod(MethodResult methodResult)
        {
            threadMethods.Add(methodResult);
        }

        internal ThreadResult(int id)
        {
            threadMethods = new List<MethodResult>();
            ThreadID = id;
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
        private Dictionary<int, ThreadResult> threadResults;
        private readonly object threadLock;

        public Dictionary<int, ThreadResult> ThreadResults
        { get => new Dictionary<int, ThreadResult>(threadResults); }

        internal ThreadResult AddThreadResult(int id)
        {
            lock(threadLock)
            {
                ThreadResult threadResult;
                if (!threadResults.TryGetValue(id, out threadResult))
                {
                    threadResult = new ThreadResult(id);
                    ThreadResults.Add(id, threadResult);
                }
                return threadResult;
            }
        }

        internal TraceResult()
        {
            threadLock = new object();
            threadResults = new Dictionary<int, ThreadResult>();
        }
    }
}