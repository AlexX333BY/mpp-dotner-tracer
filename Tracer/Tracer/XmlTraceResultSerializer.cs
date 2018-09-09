using System.IO;
using System.Xml;
using System.Runtime.Serialization;

namespace Tracer
{
    public class XmlTraceResultSerializer : ITraceResultSerializer
    {
        public Stream Stream { protected get; set; }
        protected readonly XmlWriterSettings xmlWriterSettings;
        protected readonly DataContractSerializer xmlSerializer;

        public void SerializeTraceResult(TraceResult traceResult)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(Stream, xmlWriterSettings))
            {
                xmlSerializer.WriteObject(xmlWriter, traceResult);
            }
        }

        public XmlTraceResultSerializer()
        {
            Stream = null;
            xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlSerializer = new DataContractSerializer(typeof(TraceResult));
        }

        public XmlTraceResultSerializer(Stream stream)
            : this()
        {
            Stream = stream;
        }
    }
}
