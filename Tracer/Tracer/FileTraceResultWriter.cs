using System.IO;

namespace Tracer
{
    class FileTraceResultWriter : ITraceResultWriter
    {
        public ITraceResultSerializer Serializer
        { set; protected get; }

        public string Filename
        { set; get; }

        public void Write(TraceResult traceResult)
        {
            using (Serializer.Stream = new FileStream(Filename, FileMode.Append))
            {
                Serializer.SerializeTraceResult(traceResult);
            }
        }

        public FileTraceResultWriter()
        {
            Serializer = null;
            Filename = null;
        }

        public FileTraceResultWriter(ITraceResultSerializer serializer)
        {
            Serializer = serializer;
            Filename = null;
        }

        public FileTraceResultWriter(string fileName)
        {
            Serializer = null;
            Filename = fileName;
        }

        public FileTraceResultWriter(ITraceResultSerializer serializer, string fileName)
        {
            Serializer = serializer;
            Filename = fileName;
        }
    }
}
