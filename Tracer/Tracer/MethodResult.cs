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

        [DataMember(Name = "name")]
        public string MethodName
        { get; internal set; }

        [DataMember(Name = "class")]
        public string ClassName
        { get; internal set; }

        public long Time
        {
            get => stopWatch.ElapsedMilliseconds;
        }

        [DataMember(Name = "time")]
        public string TimeWithPostfix
        {
            get => Time.ToString() + "ms";
            private set { } // to allow serialization
        }

        [DataMember(Name = "methods")]
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
