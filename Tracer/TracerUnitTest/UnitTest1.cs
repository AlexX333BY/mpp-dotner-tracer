using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;
using Tracer;

namespace TracerUnitTest
{
    [TestClass]
    public class TracerUnitTest
    {
        private static Tracer.Tracer tracer;
        private static readonly int waitTime = 100;
        private static readonly int threadsCount = 4;

        private void BoundedAssert(long actual, long expected)
        {
            const double error = 1.05;
            Assert.IsTrue((actual >= expected) && (actual <= (expected * error)));
        }

        private void SingleThreadedMethod()
        {
            tracer.StartTrace();
            Thread.Sleep(waitTime);
            tracer.StopTrace();
        }

        private void MultiThreadedMethod()
        {
            var threads = new List<Thread>();
            Thread newThread;
            for (int i = 0; i < threadsCount; i++)
            {
                newThread = new Thread(SingleThreadedMethod);
                threads.Add(newThread);
            }
            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            tracer.StartTrace();
            Thread.Sleep(waitTime);
            tracer.StopTrace();
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        [TestMethod]
        public void SingleThreadTest()
        {
            tracer = new Tracer.Tracer();
            tracer.StartTrace();
            Thread.Sleep(waitTime);
            tracer.StopTrace();
            long actual = tracer.GetTraceResult().ThreadResults[0].Time;
            BoundedAssert(actual, waitTime);
        }

        [TestMethod]
        public void MultiThreadTest()
        {
            tracer = new Tracer.Tracer();
            var threads = new List<Thread>();
            long expected = 0;
            Thread newThread;
            for (int i = 0; i < threadsCount; i++)
            {
                newThread = new Thread(SingleThreadedMethod);
                threads.Add(newThread);
                newThread.Start();
                expected += waitTime;
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            long actual = 0;
            foreach (ThreadResult threadResult in tracer.GetTraceResult().ThreadResults)
            {
                actual += threadResult.Time;
            }
            BoundedAssert(actual, expected);
        }

        [TestMethod]
        public void TwoLevelMultiThreadTest()
        {
            tracer = new Tracer.Tracer();
            var threads = new List<Thread>();
            long expected = 0;
            Thread newThread;
            for (int i = 0; i < threadsCount; i++)
            {
                newThread = new Thread(MultiThreadedMethod);
                threads.Add(newThread);
                newThread.Start();
                expected += waitTime * (threadsCount + 1);
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            long actual = 0;
            foreach (ThreadResult threadResult in tracer.GetTraceResult().ThreadResults)
            {
                actual += threadResult.Time;
            }
            BoundedAssert(actual, expected);
        }
    }
}
