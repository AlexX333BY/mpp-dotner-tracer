using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Tracer
{
    [DataContract(Name = "result")]
    public class TraceResult
    {
        private SortedDictionary<int, ThreadResult> threadResults;
        private readonly object threadLock;

        [DataMember(Name = "threads")]
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
