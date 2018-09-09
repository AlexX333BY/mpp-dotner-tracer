using System;

namespace Tracer
{
    public class Tracer : ITracer
    {
        protected bool isWorking;

        public bool IsWorking
        {
            get => isWorking;
            protected set => isWorking = value;
        }

        public TraceResult GetTraceResult()
        {
            if (IsWorking)
            {
                throw new TracerIsBusyException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void StartTrace()
        {
            if (IsWorking)
            {
                throw new TracerIsBusyException();
            }
            else
            {
                IsWorking = true;
                throw new NotImplementedException();
            }
        }

        public void StopTrace()
        {
            IsWorking = false;
            throw new NotImplementedException();
        }

        public Tracer()
        {
            isWorking = false;
        }
    }
}