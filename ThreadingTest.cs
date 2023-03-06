using System;
using System.Threading;
using System.Collections;

public class ThreadingTest
{

    private int[] _testArray;
    private Mutex _mutex = new Mutex();
    private int ThreadCount = 2;
    private int FinishedThreads = 0;

    private const int MaxLoopValue = 100000000;

    long milliSeconds;

    public void StartThreading()
    {

        _testArray = new int[MaxLoopValue];

        float threadLerp = (float)1/(float)this.ThreadCount;
        float threadMaxLerp = (float)1+1 >= this.ThreadCount ? 1 : (float)1+1/this.ThreadCount;


        Console.WriteLine("Thread lerp : " + threadLerp);
        Console.WriteLine("Thread max lerp : " + threadMaxLerp);
        // test threading with loops with call backs

        //Example ; create an 5 different threads using loops millions of number when it hits last one of the thread. Callback an function
        // With threading. Write an numbers on screen by THREAD : -- NUM -- : NUMBER TO WRITE after that write an DATA THREAD : -- NUM -- 

        var threadLineerValue = MaxLoopValue/ThreadCount;

        milliSeconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        for(int i = 0; i < ThreadCount; i++)
        {
            var threadStartCount = Lerp(0, MaxLoopValue, (float)i/(float)ThreadCount);

            Console.WriteLine("-------------------");
            Console.WriteLine("Thread Value : " + (i+1));
            Console.WriteLine("Thread Start Count : " + threadStartCount);
            Console.WriteLine("Thread End Count : " + (threadStartCount+threadLineerValue));

            var thread1 = new Thread(() => ThreadLoop("Thread : -- " + (i+1), OnThreadFinished, threadStartCount, threadStartCount + threadLineerValue));
            thread1.Start();
        }

    }

    private void ThreadLoop(string threadCount, Action action, int loopStartCount, int loopEndCount)
    {
        for(int i = loopStartCount; i < loopEndCount; i++)
        {
            _testArray[i] = i;
            //Console.WriteLine("Thread : -- " +threadCount+ " -- : Added To List  ::   LOOP VALUE : : " + i);
            
        }
        
        action.Invoke();
    }
    private void OnThreadFinished()
    {
        FinishedThreads += 1;

        if(FinishedThreads >= ThreadCount)
        {
            OnAllThreadFinished();
        }
    }

    private void OnAllThreadFinished()
    {
        Console.WriteLine("All threads finished");
        Console.WriteLine("List count : "+ _testArray.Length);

        Console.WriteLine("Time passed ; "+  (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond-milliSeconds) * .001f);


        for(int i = 0; i < _testArray.Length; i++)
        {
            Console.WriteLine("array["+i+"] : " + _testArray[i]);
        }

    }

    private int Lerp(float a, float b, float t) 
    {
        
        float output = (a + (b - a) * t);
        
        output = output > b ? b : output;
        output = output < a ? a : output;

        return (int)output;
    }
}