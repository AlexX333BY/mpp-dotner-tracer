using System.IO;

namespace Tracer
{
    public class FileTraceResultWriter : ITraceResultWriter
    {
        public string Filename
        { set; get; }

        public void Write(TraceResult traceResult, ITraceResultSerializer serializer)
        {
            using (FileStream fileStream = new FileStream(Filename, FileMode.Append))
            {
                serializer.SerializeTraceResult(traceResult, fileStream);
            }
        }

        public FileTraceResultWriter(string fileName)
        {
            Filename = fileName;
        }
    }
}
