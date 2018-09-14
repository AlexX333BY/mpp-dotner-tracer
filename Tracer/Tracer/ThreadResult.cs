using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tracer
{
    [DataContract(Name = "thread")]
    public class ThreadResult
    {
        private Stack<MethodResult> threadMethods;
        private List<MethodResult> tracedMethods;

        [DataMember(Name = "id", Order = 0)]
        public int ThreadID
        { get; internal set; }

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
        }

        [DataMember(Name = "time", Order = 1)]
        public string TimeWithPostfix
        {
            get => Time.ToString() + "ms";
            private set { } // to allow serialization
        }

        [DataMember(Name = "methods", Order = 2)]
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
}
