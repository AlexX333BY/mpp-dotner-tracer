using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Tracer
{
    [DataContract(Name = "result")]
    public class TraceResult
    {
        private ConcurrentDictionary<int, ThreadResult> threadResults;

        [DataMember(Name = "threads")]
        public List<ThreadResult> ThreadResults
        {
            get => new List<ThreadResult>(new SortedDictionary<int, ThreadResult>(threadResults).Values);
            private set { } // to allow serialization
        }

        internal ThreadResult AddOrGetThreadResult(int id)
        {
            ThreadResult threadResult;
            if (!threadResults.TryGetValue(id, out threadResult))
            {
                threadResult = new ThreadResult(id);
                threadResults[id] = threadResult;
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
            threadResults = new ConcurrentDictionary<int, ThreadResult>();
        }
    }
}
