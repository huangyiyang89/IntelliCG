using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntelliCG.Extensions;
using IntelliCG.Scripts;

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

        public override void Stop()
        {
            if (GuoZi.Cg == Cg)
            {
                GuoZi.Cg = null;
            }
            base.Stop();
        }

        public async Task<bool> Produce(Production production)
        {
            //await Cg.Actions.DecodeSend(@"{/G 9 ");
            var rawIndexs = new List<int>();
            foreach (var raw in production.Raws)
            {
                //搜索材料
                var found = Cg.Items.FindExactIndex(raw.Key, raw.Value);
                if (found <= 0)
                {
                    if (IsCloth(raw.Key)&&CanBuyCloth())
                    {
                        BuyCloth(raw.Key);
                        OnInfo($@"材料不足，购买{raw.Key}。");
                        await Task.Delay(2000);
                        return false;
                    }
                    else if(raw.Key=="砂糖"&&Cg.MoveScript.MatchIncludeTarget(MoveScript.WeiNuoYaTang))
                    {
                        OnInfo($@"材料不足，去买{raw.Key}。");
                        await BuyTang();
                        return false;
                    }
                    else
                    {
                        OnInfo($@"材料不足，缺少{raw.Key}。");
                        //Stop();
                        return false;
                    }
                }
                OnInfo($@"找到材料{raw.Key}，位置{found}。");
                rawIndexs.Add(found);
            }



            var callStr = "-C6 " + SpellIndex + " 0 -1 " + CurrentProductionId;
            var gem = Cg.Items.GetOneGem();
            if (gem != null)
            {
                callStr += "|" + gem.Index8;
                OnInfo($@"加入宝石{gem.Name}");
            }
            callStr = rawIndexs.Aggregate(callStr, (current, raw) => current + ("|" + raw)) + " ";


            OnInfo("生产中...");
            Console.WriteLine(callStr);
            await Cg.Actions.DecodeSend(@"{/G 9 ");
            await Task.Delay(17000);
            await Cg.Actions.DecodeSend(callStr);
            //await Cg.Actions.DecodeSend(@"+hV 0 ");
            OnInfo("制造完成。");
            await Task.Delay(2000);

            return true;
        }


        /// <summary>
        /// 麻布 木棉 毛毡 绵 细线
        /// </summary>
        /// <param name="index"></param>
        public async Task BuyCloth(int index)
        {
            await Cg.Actions.MoveTo(7, 7);
            await Cg.Actions.RightClick(7, 5);
            await Task.Delay(500);
            await Cg.Actions.DecodeSend($@"e74 {Cg.Player.X.To62String()} {Cg.Player.Y.To62String()} 5p {Memo.ReadInt(0x00CE42CC).To62String()} 0 {index}\\z20 ");
            await Task.Delay(1000);
        }

        public async void BuyCloth(string cloth)
        {
            switch (cloth)
            {
                case "麻布":
                    await BuyCloth(0);
                    break;
                case "木棉布":
                    await BuyCloth(1);
                    break;
                case "毛毡":
                    await BuyCloth(2);
                    break;
                case "绵":
                    await BuyCloth(3);
                    break;
                case "细线":
                    await BuyCloth(4);
                    break;
                default:
                    break;
            }
        }

        public bool IsCloth(string name)
        {
            return name == "麻布" || name == "木棉布"||name=="毛毡"||name=="绵"||name=="细线";
        }

        public bool CanBuyCloth()
        {
            return Cg.Player.MapId == 1162;
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

            if (production.Name == "寿喜锅"&&GuoZi.Cg==null)
            {
                OnInfo("制造锅子，邮寄将平衡材料。");
                GuoZi.Cg = Cg;
            }

            if (Cg.Player.Mp < production.Cost)
            {
                OnInfo("魔法不足。");
                if (Cg.Player.MapId == 1112)
                {
                   await Cg.Actions.MoveTo(8, 31);
                   await Cg.Actions.RightClick(8,30);
                }
                if (Cg.Player.MapId == 2110)
                {
                    await Cg.Actions.MoveTo(11, 5);
                    await Cg.Actions.RightClick(13,5);
                }

                if (Cg.MoveScript.MatchIncludeTarget(MoveScript.WeiNuoYaNurse))
                {
                    await Cg.MoveScript.GoTo(MoveScript.WeiNuoYaNurse);
                }
            
                return;
            }
            OnInfo("整理物品。");
            await Cg.Items.Arrange();
            await Task.Delay(1000);
            await Produce(production);
        }



        public int SpellIndex { get; set; }

        private Production GetProductionById(int id)
        {

            const int baseAddr = 0x00E16AD4;
            const int offset = 0x49FC;
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

        public async Task  BuyTang()
        {

            if (Cg.Player.MapId == 2113)
            {
                OnInfo(@"买糖。");
                var s=Cg.Stuffs.FindXiuGe();
                await Cg.Actions.MoveTo(s.X-1, s.Y);
                await Task.Delay(1000);
                await Cg.Actions.RightClick(s.X, s.Y);
                await Task.Delay(500);
                await Cg.Actions.DecodeSend($@"e74 {Cg.Player.X.To62String()} {Cg.Player.Y.To62String()} 5n {Memo.ReadInt(0x00CE42CC).To62String()} 0 1 ");
                await Task.Delay(500);
                await Cg.Actions.DecodeSend($@"e74 {Cg.Player.X.To62String()} {Cg.Player.Y.To62String()} 5p {Memo.ReadInt(0x00CE42CC).To62String()} 0 0\\z20 ");
                await Task.Delay(500);
                await Cg.Actions.DecodeSend($@"e74 {Cg.Player.X.To62String()} {Cg.Player.Y.To62String()} 5p {Memo.ReadInt(0x00CE42CC).To62String()} 0 0\\z20 ");
            }
            else
            {
                await Cg.MoveScript.GoTo(MoveScript.WeiNuoYaTang);
            }
        }

        public Producer(CrossGate cg) : base(cg)
        {
        }
    }
}
