using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.Extensions;
using IntelliCG.Items;
using IntelliCG.Pets;

namespace IntelliCG.Produces
{
    public class Poster : TaskBase
    {
        //S_K 3 1 9 123123 0     S_K 3 0 a 123123 0     名片第几个  第几个宠物  第几个物品      
        public Poster(CrossGate cg) : base(cg)
        {
        }

        public override async Task DoOnce()
        {
            if (Cg.Player.IsMoving)
            {
                return;

            }
            foreach (var pet in Cg.Pets.Posters)
            {
                if (!pet.Name.Contains('-'))
                {
                    continue;
                }

                var temp = pet.Name.Split('-');
                if (temp.Count() < 2)
                {
                    continue;
                }
                var itemName = temp[0];
                var contactName = temp[1];
                var recipient = Cg.Contacts.FindExact(contactName);

                if (recipient == null) continue;
                if (!recipient.Online)
                {
                    continue;
                }
                if (Cg.Player.Line != recipient.Line)
                {
                    OnInfo("注意，邮寄对象不在同一线。" + Cg.Player.Line + "," + recipient.Line);
                    continue;
                }

                var item = Cg.Items.FindExactFull(itemName);
                if (item == null || !recipient.Online || !item.IsFull) continue;

                if ((item.Name == "盐" || item.Name == "酱油" || item.Name == "葱" || item.Name == "牛肉") && GuoZi.Cg != null)
                {
                    if (GuoZi.Cg.Items.All.Count(i => i.Name == item.Name) >= 2)
                    {
                        //OnInfo($@"{item.Name}过多，稍后邮寄。");
                        continue;
                    }
                }


                var str = @"S_K " + recipient.Index + " " + pet.Index + " " + item.Index8.To62String() + " 来自IntelliCG自动邮寄 0 ";
                Console.WriteLine(str);
                await Cg.Actions.DecodeSend(str);
                OnInfo($@"给{recipient.Name}邮寄了{item.Name}({item.Count})。");
                await Task.Delay(2000);

            }

            var nearbyPosters = Cg.Stuffs.ReadRange(Cg.Player.X, Cg.Player.Y, 1);
            foreach (var poster in nearbyPosters)
            {
                await Cg.Actions.RightClick(poster.X, poster.Y);
                await Task.Delay(2000);
            }
        }


        public override void Start()
        {
            _petsCount = Cg.Pets.Count;
            base.Start();
        }
        public override void Stop()
        {
            if (Cg.Pets.Count < _petsCount)
            {
                OnInfo($@"停止自动邮寄，注意当前可能有宠物尚未返回。");
            }
            base.Stop();
        }

        private int _petsCount;




    }
}
