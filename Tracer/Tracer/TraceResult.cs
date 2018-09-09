using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Diagnostics;


namespace Tracer
{
    [DataContract]
    public class ThreadResult
    {
        private Stack<MethodResult> threadMethods;
        private List<MethodResult> tracedMethods;

        [DataMember]
        public int ThreadID
        { get; internal set; }

        [DataMember]
        public long Time
        {
            get
            {
                long time = 0;
                foreach (MethodResult methodResult in tracedMethods)
                {
                    time += methodResult.Time;
                }
                return time;
            }
            private set { } // to allow serialization
        }

        [DataMember]
        public List<MethodResult> InnerMethods
        {
            get => new List<MethodResult>(tracedMethods);
            private set { } // to allow serialization
        }

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

    [DataContract]
    public class MethodResult
    {
        private List<MethodResult> innerMethods;
        private Stopwatch stopWatch;

        [DataMember]
        public string MethodName
        { get; internal set; }

        [DataMember]
        public string ClassName
        { get; internal set; }

        [DataMember]
        public long Time
        {
            get => stopWatch.ElapsedMilliseconds;
            private set { } // to allow serialization
        }

        [DataMember]
        public List<MethodResult> InnerMethods
        {
            get => new List<MethodResult>(innerMethods);
            private set { } // to allow serialization
        }

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

    [DataContract]
    public class TraceResult
    {
        private SortedDictionary<int, ThreadResult> threadResults;
        private readonly object threadLock;

        [DataMember]
        public List<ThreadResult> ThreadResults
        {
            get => new List<ThreadResult>(threadResults.Values);
            private set { } // to allow serialization
        }

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

        /// <exception cref="KeyNotFoundException">Thrown if there no thread with specified ID</exception>
        internal ThreadResult GetThreadResult(int id)
        {
            return threadResults[id];
        }

        internal TraceResult()
        {
            threadLock = new object();
            threadResults = new SortedDictionary<int, ThreadResult>();
        }
    }
}