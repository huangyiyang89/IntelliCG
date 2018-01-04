using IntelliCG.MemoryHelper;

namespace IntelliCG.Cheat
{
    public class Cheat:Base
    {
        public Cheat(Memo memo):base(memo)
        {
        }
        
        public bool GaoSuZhanDou
        {
            get=> Memo.ReadBytes(0x0450756, 2) == "90-90";
            set => Memo.WriteBytes(0x0450756, value ? "90-90" : "75-0B");
        }

        public bool BuBuYuDi
        {
            get => Memo.ReadBytes(0x04865C9, 2) == "90-90";
            set
            {
                if (value)
                {
                    Memo.WriteBytes(0x04865C9, "90-90");
                    Memo.WriteInt(0x04865D1, 2);
                }
                else
                {
                    Memo.WriteBytes(0x04865C9, "EB-56");
                    Memo.WriteInt(0x04865D1, 3);
                }
            }
        }

        public bool ZhanDouLiaoLi
        {
            get => Memo.ReadBytes(0x004B33E2,1) == "EB";
            set
            {
                if (value)
                {
                    Memo.WriteBytes(0x004B33E2, "EB");
                    Memo.WriteBytes(0x004B2FCF, "90-90-90-90-90-90");
                }
                else
                {
                    Memo.WriteBytes(0x004B33E2, "75");
                    Memo.WriteBytes(0x004B2FCF, "0F-84-D5-00-00-00");
                }
            }
        }

        public bool CaiJiJiaSu
        {
            get => Memo.ReadInt(0x04077E5) == 3000;
            set => Memo.WriteInt(0x04077E5, value ? 3000 : 4500);
            
        }

        public bool YiDongJiaSu
        {
            get => Memo.ReadInt(Memo.GetPointer(0x010AEF88)+0x168) == 300;
            set => Memo.WriteInt(Memo.GetPointer(0x010AEF88) + 0x168, value ? 300 : 100);
        }
        


    }
}
