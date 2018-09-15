using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace Tracer
{
    public class XmlTraceResultSerializer : ITraceResultSerializer
    {
        protected readonly XmlWriterSettings xmlWriterSettings;
        protected readonly DataContractSerializer xmlSerializer;

        public void SerializeTraceResult(TraceResult traceResult, Stream stream)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
            {
                xmlSerializer.WriteObject(xmlWriter, traceResult);
            }
        }

        public XmlTraceResultSerializer()
        {
            xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true
            };
            xmlSerializer = new DataContractSerializer(typeof(TraceResult));
        }
    }
}
