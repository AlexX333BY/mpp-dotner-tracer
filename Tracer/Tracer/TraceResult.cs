using System.Collections.Generic;
using System.Diagnostics;

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

        protected void AddThreadMethod(MethodResult methodResult)
        {
            if (threadMethods.Count > 0)
            {
                threadMethods.Peek().AddInnerMethod(methodResult);
            }
            else
            {
                tracedMethods.Add(methodResult);
            }
            threadMethods.Push(methodResult);
        }

        internal void StartTracingMethod(MethodResult methodResult)
        {
            AddThreadMethod(methodResult);
            methodResult.StartTrace();
        }

        internal void StopTracingMethod()
        {
            if (threadMethods.Count == 0)
            {
                throw new NoMethodTracingException(ThreadID);
            }
            threadMethods.Pop().StopTrace();
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
        private Stopwatch stopWatch;

        public string MethodName
        { get; internal set; }
        public string ClassName
        { get; internal set; }
        public string Time
        { get => stopWatch.ElapsedMilliseconds.ToString() + "ms"; }
        public List<MethodResult> InnerMethods
        { get => new List<MethodResult>(innerMethods); }

        internal void StartTrace()
        {
            stopWatch.Start();
        }

        internal void StopTrace()
        {
            stopWatch.Stop();
        }

        internal void AddInnerMethod(MethodResult methodResult)
        {
            innerMethods.Add(methodResult);
        }

        internal MethodResult()
        {
            innerMethods = new List<MethodResult>();
            stopWatch = new Stopwatch();
        }
    }

    public class TraceResult
    {
        private SortedDictionary<int, ThreadResult> threadResults;
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
            threadResults = new SortedDictionary<int, ThreadResult>();
        }
    }
}