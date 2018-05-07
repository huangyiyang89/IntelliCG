using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntelliCG.Produces
{
    public class Producer : TaskBase
    {


        public bool Complete => Cg.Memo.ReadInt(0x00CC41AA) == 2;
        public bool Producing => Cg.Memo.ReadInt(0x00CC41AA) == 1;

        //未打开窗口返回0
        private bool IsOpeningWindow => Memo.ReadInt(0x00D56F48) != 0;
        private int CurrentProductionId => Memo.ReadInt(0x00D4C830);

        
        private int _productionId;


        public override void Start()
        {
            _productionId = CurrentProductionId;
            if (_productionId == 0)
            {
                OnInfo(@"先打开要生产物品的窗口,之后可以关闭。");
                Stop();
                return;
            }
            
            base.Start();
        }

        public void Start(ProductionId productionId)
        {
            _productionId = (int)productionId;
            base.Start();
        }

        public async Task<bool> Produce(Production production)
        {
            //await Cg.Actions.DecodeSend(@"{/G 9 ");
            var rawIndexs = new List<int>();
            foreach (var raw in production.Raws)
            {
                //搜索材料
                var found = Cg.Items.FindExactIndex(raw.Key, raw.Value);
                if (found < 0)
                {
                    OnInfo($@"材料不足，缺少{raw.Key}。");
                    Stop();
                    return false;
                }
                OnInfo($@"找到材料{raw.Key}，位置{found}。");
                rawIndexs.Add(found + 8);
            }

           

            var callStr = "-C6 " + SpellIndex + " 0 -1 " + CurrentProductionId;
            var gem = Cg.Items.GetOneGem();
            if(gem!=null)
            {
                callStr += "|"+gem.Index8;
                OnInfo($@"加入宝石{gem.Name}");
            }
            callStr = rawIndexs.Aggregate(callStr, (current, raw) => current + ("|" + raw))+" ";

           
            OnInfo("生产中...");
            Console.WriteLine(callStr);
            await Cg.Actions.DecodeSend(@"{/G 9 ");
            await Task.Delay(12000);
            await Cg.Actions.DecodeSend(callStr);
            //await Cg.Actions.DecodeSend(@"+hV 0 ");
            OnInfo("制造完成。");
            await Task.Delay(2000);
            
            return true;
        }


        public override async Task DoOnce()
        {
            var production = GetProductionById(_productionId);
            if (production == null)
            {
                OnInfo("错误！未找到配方。");
                Stop();
                return;
            }
            if (Cg.Player.Mp < production.Cost)
            {
                OnInfo("魔法不足。");
                Stop();
                return;
            }
            await Cg.Items.Arrange();
            await Produce(production);
        }

        

        public int SpellIndex { get; set; }

        private Production GetProductionById(int id)
        {
            
            const int baseAddr = 0x00E16AD4;
            const int offset=0x49FC;
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 38; j++)
                {
                    var readId = Memo.ReadInt(baseAddr + i * offset + j * 0x134);
                    if (readId != id) continue;
                    SpellIndex = i;
                    var production = new Production
                    {
                        Cost = Memo.ReadInt(baseAddr + i * offset + j * 0x134 + 0x4),
                        Name = Memo.ReadString(baseAddr + i * offset + j * 0x134 + 0x14, 40),
                        Raws = new Dictionary<string, int>()
                    };
                    for (var k = 0; k < 5; k++)
                    {
                        var rawCount = Memo.ReadInt(baseAddr + i * offset + j * 0x134 + 0x70 + 0x28 * k);
                        var rawName = Memo.ReadString(baseAddr + i * offset + j * 0x134 + 0x74 + 0x28 * k, 40);
                        if (rawName != "")
                        {
                            production.Raws.Add(rawName, rawCount);
                        }
                    }
                    return production;
                }
            }
            return null;
        }


        public Producer(CrossGate cg) : base(cg)
        {
        }
    }
}
