using System;
using System.Collections.Generic;
using System.Threading;
using Tracer;

namespace TracerUsageExample
{
    internal class ExampleMethods
    {
        private static ITracer _tracer;

        internal void SimpleMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(new Random().Next(100, 1000));
            _tracer.StopTrace();
        }

        internal void NotSoSimpleMethod()
        {
            _tracer.StartTrace();
            SimpleMethod();
            SimpleMethod();
            Thread.Sleep(new Random().Next(100, 1000));
            _tracer.StopTrace();
        }

        internal void AbsolutelyNotSimpleMethod()
        {
            _tracer.StartTrace();
            SimpleMethod();
            NotSoSimpleMethod();
            NotSoSimpleMethod();
            Thread.Sleep(new Random().Next(100, 1000));
            _tracer.StopTrace();
        }

        internal void MultiThreadedMethod()
        {
            _tracer.StartTrace();
            var threads = new List<Thread>();
            threads.Add(new Thread(SimpleMethod));
            threads.Add(new Thread(NotSoSimpleMethod));
            threads.Add(new Thread(AbsolutelyNotSimpleMethod));
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            Thread.Sleep(new Random().Next(100, 1000));
            _tracer.StopTrace();
        }

        internal ExampleMethods(ITracer tracer)
        {
            _tracer = tracer;
        }
    }

    class Example
    {
        private static Tracer.Tracer tracer;

        static void Main(string[] args)
        {
            tracer = new Tracer.Tracer();
            new ExampleMethods(tracer).MultiThreadedMethod();
            ITraceResultSerializer serializer = new JsonTraceResultSerializer();
            ITraceResultWriter writer = new ConsoleTraceResultWriter();
            writer.Serializer = serializer;
            writer.Write(tracer.GetTraceResult());
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            serializer = new XmlTraceResultSerializer();
            writer = new FileTraceResultWriter(serializer, desktopPath + "\\xmlSerialized.xml");
            writer.Write(tracer.GetTraceResult());
            Console.ReadKey();
        }
    }
}
