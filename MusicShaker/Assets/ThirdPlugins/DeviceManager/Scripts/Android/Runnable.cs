namespace DeviceManager
{
    using System;
    using UnityEngine;
    public interface Runnable
    {
        void run();
    }

    public delegate void Run();

    public class AndroidRunnable : AndroidJavaProxy, Runnable
    {
        Run _run;
        public AndroidRunnable(Run run) : base("java.lang.Runnable")
        {
            _run = run;
        }

        public void run()
        {
            _run();
        }
    }
}