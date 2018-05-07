using System;
using MemoLib;

namespace IntelliCG.Cheat
{
    public class Cheater:Base
    {
        public Cheater(Memo memo):base(memo)
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
            //[010AEF88]+168
            get => Memo.ReadBytes(0x00488FCE,6) == "8B-88-68-01-00-00";

            set
            {
                Memo.WriteBytes(0x00488FCE, value ? "B9 00 02 00 00 90" : "8B-88-68-01-00-00");
                Memo.WriteBytes(0x0048609E, value ? "B9 00 02 00 00 90" : "8B-88-68-01-00-00");
            }
             
        }

        public bool ErDongJiNeng
        {
            get => Memo.ReadBytes(0x004CD79D,6)=="90-90-90-90-90-90";
            set => Memo.WriteBytes(0x004CD79D, value ? "90-90-90-90-90-90" : "89-1D-34-56-64-00");
        }


        public void CloseMutant()
        {
           
        }

        public bool SuperSpeed
        {
            get => !(Memo.ReadBytes(0x005F2FA0,8)=="AB-AA-AA-AA-AA-AA-30-40"&&
                   Memo.ReadBytes(0x005F2F98,8)=="AB-AA-AA-AA-AA-AA-30-40"&&
                   Memo.ReadBytes(0x005C8328,8)=="AB-AA-AA-AA-AA-AA-30-40");
            set
            {
                if (value)
                {
                    Memo.ChangeProtect(0x005C8328,8);
                    Memo.WriteDouble(0x005C8328, 2.0);
                    Memo.WriteDouble(0x005F2FA0, 2.0);
                    Memo.WriteDouble(0x005F2F98, 2.0);
                    
                }
                else
                {
                    Memo.WriteBytes(0x005C8328, "AB-AA-AA-AA-AA-AA-30-40");
                    Memo.WriteBytes(0x005F2FA0, "AB-AA-AA-AA-AA-AA-30-40");
                    Memo.WriteBytes(0x005F2F98, "AB-AA-AA-AA-AA-AA-30-40");
                }
            }
        }

        ~Cheater()
        {
            Console.WriteLine(@"~Cheater()");
        }

        public void Close()
        {
            Console.WriteLine(@"Cheater.Close()");
            GaoSuZhanDou = false;
            BuBuYuDi = false;
            ZhanDouLiaoLi = false;
            CaiJiJiaSu = false;
            YiDongJiaSu = false;
            ErDongJiNeng = false;
            SuperSpeed = false;
        }
    }
}
