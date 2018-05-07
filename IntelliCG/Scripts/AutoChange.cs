using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.Extensions;

namespace IntelliCG.Scripts
{
    public class AutoChange:TaskBase
    {
        public AutoChange(CrossGate cg) : base(cg)
        {
        }


        public async Task Change(int count)
        {
            await Cg.Actions.DecodeSend($@"e74 {Cg.Player.X.To62String()} {Cg.Player.Y.To62String()} 5y {Memo.ReadInt(0x00CE42CC).To62String()} 0 1 ");
            await Task.Delay(1000);
            await Cg.Actions.DecodeSend($@"e74 {Cg.Player.X.To62String()} {Cg.Player.Y.To62String()} 5z {Memo.ReadInt(0x00CE42CC).To62String()} 0 0\\z{count} ");
        }

        public override async Task DoOnce()
        {
            var count=0;
            count = Cg.Items.All.Count(i => i.Name == "纯银" && i.Count == 20);
            if ( count>= 1)
            {

               await Cg.Actions.MoveTo(26,5);
               await Cg.Actions.RightClick(27, 6);
               await Change(count);
               await Task.Delay(1000);
               await Cg.Items.Arrange();
            }

            count = Cg.Items.All.Count(i => i.Name == "铜" && i.Count == 20);
            if ( count>= 1)
            {
                await Cg.Actions.MoveTo(26,5);
                await Cg.Actions.RightClick(26, 4);
                await Change(count);
                await Task.Delay(1000);
                await Cg.Items.Arrange();
            }

            count = Cg.Items.All.Count(i => i.Name == "铁" && i.Count == 20);
            if ( count>= 1)
            {
                await Cg.Actions.MoveTo(26,5);
                await Cg.Actions.RightClick(28, 5);
                await Change(count);
                await Task.Delay(1000);
                await Cg.Items.Arrange();
            }


            count = Cg.Items.All.Count(i => i.Name == "银" && i.Count == 20);
            if ( count>= 1)
            {
                await Cg.Actions.MoveTo(29,6);
                await Cg.Actions.RightClick(30, 5);
                await Change(count);
                await Task.Delay(1000);
                await Cg.Items.Arrange();
            }


            count = Cg.Items.All.Count(i => i.Name == "金" && i.Count == 20);
            if ( count>= 1)
            {
                await Cg.Actions.MoveTo(25,6);
                await Cg.Actions.RightClick(24, 5);
                await Change(count);
                await Task.Delay(1000);
                await Cg.Items.Arrange();
            }

            count = Cg.Items.All.Count(i => i.Name == "白金" && i.Count == 20);
            if ( count>= 1)
            {
                await Cg.Actions.MoveTo(29,6);
                await Cg.Actions.RightClick(30, 7);
                await Change(count);
                await Task.Delay(1000);
                await Cg.Items.Arrange();
            }
            await Task.Delay(2000);
        }

        
    }
}
