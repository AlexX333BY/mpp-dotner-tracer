using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.Serialization.Json;

namespace Tracer
{
    public class JsonTraceResultSerializer : ITraceResultSerializer
    {
        protected readonly DataContractJsonSerializer jsonSerializer;

        public void SerializeTraceResult(TraceResult traceResult, Stream stream)
        {
            using (XmlDictionaryWriter jsonWriter = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, ownsStream: true, indent: true))
            {
                jsonSerializer.WriteObject(jsonWriter, traceResult);
            }
        }

        public JsonTraceResultSerializer()
        {
            jsonSerializer = new DataContractJsonSerializer(typeof(TraceResult));
        }
    }
}
