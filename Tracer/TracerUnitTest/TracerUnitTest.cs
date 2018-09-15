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

        private void TimeTest(long actual, long expected)
        {
            Assert.IsTrue(actual >= expected);
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
            // only checks time
            tracer = new Tracer.Tracer();
            tracer.StartTrace();
            Thread.Sleep(waitTime);
            tracer.StopTrace();
            long actual = tracer.GetTraceResult().ThreadResults[0].Time;
            TimeTest(actual, waitTime);
        }

        [TestMethod]
        public void MultiThreadTest()
        {
            // only checks time
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
            TimeTest(actual, expected);
        }

        [TestMethod]
        public void TwoLevelMultiThreadTest()
        {
            // checks time, amount, classnames and methodnames
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
            TraceResult result = tracer.GetTraceResult();
            foreach (ThreadResult threadResult in result.ThreadResults)
            {
                actual += threadResult.Time;
            }
            TimeTest(actual, expected);
            Assert.AreEqual(threadsCount * threadsCount + threadsCount, result.ThreadResults.Count);
            int multiThreadedMethodCounter = 0, singleThreadedMethodCounter = 0;
            MethodResult methodResult;
            foreach (ThreadResult threadResult in result.ThreadResults)
            {
                Assert.AreEqual(threadResult.InnerMethods.Count, 1);
                methodResult = threadResult.InnerMethods[0];
                Assert.AreEqual(0, methodResult.InnerMethods.Count);
                Assert.AreEqual(nameof(TracerUnitTest), methodResult.ClassName);
                TimeTest(methodResult.Time, waitTime);
                if (methodResult.MethodName == nameof(MultiThreadedMethod))
                    multiThreadedMethodCounter++;
                if (methodResult.MethodName == nameof(SingleThreadedMethod))
                    singleThreadedMethodCounter++;
            }
            Assert.AreEqual(threadsCount, multiThreadedMethodCounter);
            Assert.AreEqual(threadsCount * threadsCount, singleThreadedMethodCounter);
        }

        [TestMethod]
        public void InnerMethodTest()
        {
            // checks time, amount, classnames and methodnames 
            tracer = new Tracer.Tracer();
            tracer.StartTrace();
            Thread.Sleep(waitTime);
            SingleThreadedMethod();
            tracer.StopTrace();
            TraceResult traceResult = tracer.GetTraceResult();
            
            Assert.AreEqual(1, traceResult.ThreadResults.Count);
            TimeTest(tracer.GetTraceResult().ThreadResults[0].Time, waitTime * 2);
            Assert.AreEqual(1, traceResult.ThreadResults[0].InnerMethods.Count);
            MethodResult methodResult = traceResult.ThreadResults[0].InnerMethods[0];
            Assert.AreEqual(nameof(TracerUnitTest), methodResult.ClassName);
            Assert.AreEqual(nameof(InnerMethodTest), methodResult.MethodName);
            TimeTest(methodResult.Time, waitTime * 2);
            Assert.AreEqual(1, methodResult.InnerMethods.Count);
            MethodResult innerMethodResult = methodResult.InnerMethods[0];
            Assert.AreEqual(nameof(TracerUnitTest), innerMethodResult.ClassName);
            Assert.AreEqual(nameof(SingleThreadedMethod), innerMethodResult.MethodName);
            TimeTest(innerMethodResult.Time, waitTime);
        }
    }
}
