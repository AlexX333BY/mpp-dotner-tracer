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
            // only checks time
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
            BoundedAssert(actual, expected);
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
            BoundedAssert(actual, expected);
            Assert.AreEqual(threadsCount * threadsCount + threadsCount, result.ThreadResults.Count);
            int mtmCount = 0, stmCount = 0;
            MethodResult methodResult;
            foreach (ThreadResult threadResult in result.ThreadResults)
            {
                Assert.AreEqual(threadResult.InnerMethods.Count, 1);
                methodResult = threadResult.InnerMethods[0];
                Assert.AreEqual(0, methodResult.InnerMethods.Count);
                Assert.AreEqual("TracerUnitTest", methodResult.ClassName);
                BoundedAssert(methodResult.Time, waitTime);
                if (methodResult.MethodName == "MultiThreadedMethod")
                    mtmCount++;
                if (methodResult.MethodName == "SingleThreadedMethod")
                    stmCount++;
            }
            Assert.AreEqual(threadsCount, mtmCount);
            Assert.AreEqual(threadsCount * threadsCount, stmCount);
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
            BoundedAssert(tracer.GetTraceResult().ThreadResults[0].Time, waitTime * 2);
            Assert.AreEqual(1, traceResult.ThreadResults[0].InnerMethods.Count);
            MethodResult methodResult = traceResult.ThreadResults[0].InnerMethods[0];
            Assert.AreEqual("TracerUnitTest", methodResult.ClassName);
            Assert.AreEqual("InnerMethodTest", methodResult.MethodName);
            BoundedAssert(methodResult.Time, waitTime * 2);
            Assert.AreEqual(1, methodResult.InnerMethods.Count);
            MethodResult innerMethodResult = methodResult.InnerMethods[0];
            Assert.AreEqual("TracerUnitTest", innerMethodResult.ClassName);
            Assert.AreEqual("SingleThreadedMethod", innerMethodResult.MethodName);
            BoundedAssert(innerMethodResult.Time, waitTime);
        }
    }
}
