using System;

namespace Tracer
{
    public class NoMethodTracingException : Exception
    {
        public int ThreadID
        { get; protected set; }

        public NoMethodTracingException(int id)
        {
            ThreadID = id;
        }
    }
}
