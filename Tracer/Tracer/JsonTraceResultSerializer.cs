using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization.Json;

namespace Tracer
{
    public class JsonTraceResultSerializer : ITraceResultSerializer
    {
        public Stream Stream { protected get; set; }

        public void SerializeTraceResult(TraceResult traceResult)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(TraceResult));
            XmlDictionaryWriter jsonWriter = JsonReaderWriterFactory.CreateJsonWriter(Stream, Encoding.UTF8, true, true);
            jsonSerializer.WriteObject(jsonWriter, traceResult);
        }

        public JsonTraceResultSerializer()
        {
            Stream = null;
        }

        public JsonTraceResultSerializer(Stream stream)
        {
            Stream = stream;
        }
    }
}