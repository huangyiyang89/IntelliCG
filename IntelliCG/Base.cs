using MemoLib;


namespace IntelliCG
{
    
    public abstract class Base
    {
       
        protected Base(Memo memo)
        {
            Memo = memo;
        }
        
        public Memo Memo { get; set; }

        

    }
}


