using System;
using System.Threading.Tasks;

namespace IntelliCG
{
    public abstract class TaskBase:CgBase
    {
        protected TaskBase(CrossGate cg) : base(cg)
        {
        }
        public event EventHandler Info;
        public event EventHandler Started;
        public event EventHandler Stopped;
        protected bool Enable;

        protected bool IsRunning;
        public virtual void Start()
        {
            if (IsRunning == false)
            {
                Console.WriteLine(@"Thread Start.");
                TaskMethod();
            }
        }

        
        public virtual void Stop()
        {
            if (IsRunning == false)
            {
                Stopped?.Invoke(this,EventArgs.Empty);
            }
            Enable = false;
        }

        
        protected async void TaskMethod()
        {
            Started?.Invoke(this,EventArgs.Empty);
            Enable = true;
            IsRunning = true;
            while (Enable)
            {
                await DoOnce();
                await Task.Delay(100);
            }
            IsRunning = false;
            Stopped?.Invoke(this,EventArgs.Empty);
            Console.WriteLine(@"Thread Stopped.");
        }

        public abstract Task DoOnce();
        
        protected virtual void OnInfo(string info)
        {
            Info?.Invoke(this, new MessageEventArgs(info));
        }


        
    }
}
