using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization.Json;

namespace Tracer
{
    public class JsonTraceResultSerializer : ITraceResultSerializer
    {
        public Stream Stream { protected get; set; }
        protected readonly DataContractJsonSerializer jsonSerializer;

        public void SerializeTraceResult(TraceResult traceResult)
        {
            using (XmlDictionaryWriter jsonWriter = JsonReaderWriterFactory.CreateJsonWriter(Stream, Encoding.UTF8, true, true))
            {
                jsonSerializer.WriteObject(jsonWriter, traceResult);
            }
        }

        public JsonTraceResultSerializer()
        {
            Stream = null;
            jsonSerializer = new DataContractJsonSerializer(typeof(TraceResult));
        }

        public JsonTraceResultSerializer(Stream stream)
            : this()
        {
            Stream = stream;
        }
    }
}