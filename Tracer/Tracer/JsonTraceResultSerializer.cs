using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    class JsonTraceResultSerializer : ITraceResultSerializer
    {
        protected Stream stream;

        public void SerializeTraceResult(TraceResult traceResult)
        {
            throw new NotImplementedException();
        }

        public void SetStream(Stream stream)
        {
            throw new NotImplementedException();
        }

        public JsonTraceResultSerializer()
        {
            stream = null;
        }

        public JsonTraceResultSerializer(Stream stream)
        {
            this.stream = stream;
        }
    }
}