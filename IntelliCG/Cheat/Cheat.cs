namespace IntelliCG.Cheat
{
    public class Cheat:Base
    {
        public Cheat(int hwnd):base(hwnd)
        {
        }
        
        public bool GaoSuZhanDou
        {
            get=> Dm.ReadData(Hwnd, addr: "0450756", len: 2) == "90 90";
            set => Dm.WriteData(Hwnd, "0450756", value ? "90 90" : "75 0B");
        }

        public bool BuBuYuDi
        {
            get => Dm.ReadData(Hwnd, "04865C9", 2) == "90 90";
            set
            {
                if (value)
                {
                    Dm.WriteData(Hwnd, "04865C9", "90 90");
                    Dm.WriteInt(Hwnd, "04865D1", 0, 2);
                }
                else
                {
                    Dm.WriteData(Hwnd, "04865C9", "EB 56");
                    Dm.WriteInt(Hwnd, "04865D1", 0, 3);
                }
            }
        }

        public bool ZhanDouLiaoLi
        {

            get => Dm.ReadData(Hwnd, "004B33E2", 1) == "EB";
            set
            {
                if (value)
                {
                    Dm.WriteData(Hwnd, "004B33E2", "EB");
                    Dm.WriteData(Hwnd, "004B2FCF", "90 90 90 90 90 90");
                }
                else
                {
                    Dm.WriteData(Hwnd, "004B33E2", "75");
                    Dm.WriteData(Hwnd, "004B2FCF", "0F 84 D5 00 00 00");
                }
            }
        }

        public bool CaiJiJiaSu
        {
            get=> Dm.ReadInt(Hwnd, "04077E5", 0)==4500;
            set => Dm.WriteInt(Hwnd, "04077E5", 0, value ? 4500 : 6000);
        }

        public bool YiDongJiaSu
        {
            get => Dm.ReadInt(Hwnd, "[010AEF88] + 168", 0) == 300;
            set => Dm.WriteInt(Hwnd, "[010AEF88] + 168", 0, value ? 300 : 100);
        }
        


    }
}
