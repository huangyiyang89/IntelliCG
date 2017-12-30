using Dm;

namespace IntelliCG
{
    
    public abstract class Base
    {
        protected Base(int hwnd)
        {
            Hwnd = hwnd;
            Dm.WriteInt(Hwnd, "00D5A000", 0, 0);
        }


        public int Hwnd { get; set; }
        
        protected static dmsoft Dm { get;} = new dmsoft();
    }
}


