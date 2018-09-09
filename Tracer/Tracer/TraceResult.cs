using System.Collections.Generic;

namespace Tracer
{
    public class ThreadResult
    {
        private Stack<MethodResult> threadMethods;
        private List<MethodResult> tracedMethods;

        public int ThreadID
        { get; internal set; }
        public string Time
        { get; internal set; }
        public List<MethodResult> InnerMethods
        { get => new List<MethodResult>(tracedMethods); }

        internal void AddThreadMethod(MethodResult methodResult)
        {
            if (threadMethods.Count > 0)
            {
                threadMethods.Peek().AddInnerMethod(methodResult);
            }
            threadMethods.Push(methodResult);
        }

        internal ThreadResult(int id)
        {
            threadMethods = new Stack<MethodResult>();
            tracedMethods = new List<MethodResult>();
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

        internal ThreadResult AddOrGetThreadResult(int id)
        {
            ThreadResult threadResult;
            lock(threadLock)
            {
                if (!threadResults.TryGetValue(id, out threadResult))
                {
                    threadResult = new ThreadResult(id);
                    threadResults.Add(id, threadResult);
                }
            }
            return threadResult;
        }

        internal TraceResult()
        {
            threadLock = new object();
            threadResults = new Dictionary<int, ThreadResult>();
        }
    }
}