﻿using System;

namespace Tracer
{
    public class NoMethodTracingException : Exception
    {
        public int ThreadID
        { get; protected set; }

        public NoMethodTracingException()
        {
            ThreadID = 0;
        }

        public NoMethodTracingException(int id)
        {
            ThreadID = id;
        }
    }
}
