using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CgData.Models;
using MemoLib;

namespace StallReader
{
    public class StallReader
    {


        //11格以内
        public Memo Memo { get; set; }


        public List<CgData.Models.Stall> ReadNearbyStalls()
        {
            const int offset = 0x13C;
            var stalls = new List<CgData.Models.Stall>();
            stalls.Clear();
            for (var i = 0; i < 100; i++)
            {
                var playerName = Memo.ReadString(0x00645CC4 + i * offset, 20);
                if (playerName == "")
                {
                    break;
                }
                var isStall = Memo.ReadInt(0x00645D90 + i * offset) == 4096;
                if (!isStall)
                {
                    continue;
                }

                var stallName = Memo.ReadString(0x00645D98 + i * offset, 20);
                var x = (int)Memo.ReadFloat(Memo.GetPointer(0x645D8C + i * offset) + 0x274) / 64;
                var y = (int)Memo.ReadFloat(Memo.GetPointer(0x645D8C + i * offset) + 0x278) / 64;
                var stall = new CgData.Models.Stall()
                {
                    StallName = stallName,
                    X = x,
                    Y = y,
                    PlayerName = playerName,
                    Line = "II"
                };
                Console.WriteLine($@"{playerName},{stallName},{x},{y}");
                stalls.Add(stall);
            }
            Console.WriteLine($@"Total:{stalls.Count}");
            return stalls;
        }

        //装备  料理  药   采集材料   
        public void ReadStallItems(CgData.Models.Stall stall)
        {
            const int offset = 0x65C;
            var playerName = Memo.ReadString(0x0061D1B0, 20);
            var stallDescription = Memo.ReadString(0x61D1D2, 100);

            stall.Description = stallDescription;
            stall.UpdateTime=DateTime.Now;

            var items = new List<Item>();
            for (var i = 0; i < 100; i++)
            {
                var name = Memo.ReadString(0x0061D21E + i * offset, 20);
                if (name == "")
                {
                    break;
                }
                
                var firstLine = Memo.ReadString(0x0061D24C + i * offset, 80);
                var secondLine = Memo.ReadString(0x0061D2AC + i * offset, 80);
                var thirdLine = Memo.ReadString(0x0061D30C + i * offset, 80);
                var fouthLine = Memo.ReadString(0x0061D36C + i * offset, 80);
                var fifthLine = Memo.ReadString(0x0061D3CC + i * offset, 80);
                var sixLine = Memo.ReadString(0x0061D42C + i * offset, 80);
                var allLine = (firstLine + " " + secondLine + " " + thirdLine + " " + fouthLine + " " + fifthLine +
                               " " +
                               sixLine + " ").Replace("  ", " ");

                var naiJiu = new Regex(@"耐久.[0-9\/]+").Match(allLine).Value.Replace("种类", "");
                var dengJi = GetValue(allLine, "等级");
                var zhongLei = new Regex(@"种类.[\u4e00-\u9fa5]+").Match(allLine).Value.Replace("种类", "");
                var count = Memo.ReadInt(0x0061D860 + i * 4);
                var price = Memo.ReadInt(0x0062D07C + i * 4);

                var item = new Item
                {
                    DengJi = dengJi,
                    ZhongLei = zhongLei,
                    Price = price,
                    Count = count
                };

                Console.Write($@"{name},等级 {dengJi},种类 {zhongLei},");
                if (naiJiu == "") continue;
                //装备
                var shengMing = GetValue(allLine, "生命");
                var moLi = GetValue(allLine, "魔力");
                var gongJi = GetValue(allLine, "攻击");
                var fangYu = GetValue(allLine, "防御");
                var minJie = GetValue(allLine, "敏捷");
                var jingShen = GetValue(allLine, "精神");
                var huiFu = GetValue(allLine, "回复");
                var meiLi = GetValue(allLine, "魅力");
                var biSha = GetValue(allLine, "必杀");
                var mingZhong = GetValue(allLine, "命中");
                var fanJi = GetValue(allLine, "反击");
                var shanDuo = GetValue(allLine, "闪躲");
                var moGong = GetValue(allLine, "魔攻");
                var kangMo = GetValue(allLine, "抗魔");
                var shiHua = GetValue(allLine, "石化");
                var du = GetValue(allLine, "毒");
                var zui = GetValue(allLine, "醉");
                var hunShui = GetValue(allLine, "昏睡");
                var yiWang = GetValue(allLine, "遗忘");
                var hunLuan = GetValue(allLine, "混乱");
                item.ShengMing = shengMing;
                item.MoLi = moLi;
                item.GongJi = gongJi;
                item.FangYu = fangYu;
                item.MinJie = minJie;
                item.JingShen = jingShen;
                item.HuiFu = huiFu;
                item.BiSha = biSha;
                item.MingZhong = mingZhong;
                item.FanJi = fanJi;
                item.ShanDuo = shanDuo;
                item.MoGong = moGong;
                item.KangMo = kangMo;
                item.ShiHua = shiHua;
                item.Du = du;
                item.Zui = zui;
                item.HunShui = hunShui;
                item.YiWang = yiWang;
                item.HunLuan = hunLuan;
                Console.WriteLine(
                    $@"生命 {shengMing},魔力 {moLi},攻击 {gongJi},防御 {fangYu},敏捷 {minJie},精神 {jingShen},回复 {huiFu},魅力 {
                            meiLi
                        },必杀 {biSha},命中 {mingZhong},反击 {fanJi},闪躲 {shanDuo},魔攻 {moGong},抗磨 {kangMo},石化 {
                            shiHua
                        },毒 {du},醉 {zui},昏睡 {hunShui},遗忘 {yiWang},混乱 {hunLuan},耐久 {naiJiu}");

                items.Add(item);

            }
            stall.StallItems = items;
        }

        private int GetValue(string str, string attribute)
        {
            var value = new Regex(attribute + @"+.[0-9\.\-\+]+").Match(str).Value.Replace(attribute, "");
            return value == "" ? 0 : int.Parse(value);
        }


    }




}
