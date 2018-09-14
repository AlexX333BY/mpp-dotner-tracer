using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Tracer
{
    [DataContract(Name = "method")]
    public class MethodResult
    {
        private List<MethodResult> innerMethods;
        private Stopwatch stopWatch;

        [DataMember(Name = "name", Order = 0)]
        public string MethodName
        { get; internal set; }

        [DataMember(Name = "class", Order = 1)]
        public string ClassName
        { get; internal set; }

        public long Time
        {
            get => stopWatch.ElapsedMilliseconds;
        }

        [DataMember(Name = "time", Order = 2)]
        public string TimeWithPostfix
        {
            get => Time.ToString() + "ms";
            private set { } // to allow serialization
        }

        [DataMember(Name = "methods", Order = 3)]
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
}
