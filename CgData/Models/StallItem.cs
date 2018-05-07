

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CgData.Models
{
    public class StallItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int DengJi { get; set; }
        public string ZhongLei { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public int NaiJiu { get; set; }
        public int NaiJiuMax { get; set; }
        public int ShengMing { get; set; }
        public int MoLi { get; set; }
        public int GongJi { get; set; }
        public int FangYu { get; set; }
        public int MinJie { get; set; }
        public int JingShen { get; set; }
        public int HuiFu { get; set; }
        public int MeiLi { get; set; }
        public int BiSha { get; set; }
        public int MingZhong { get; set; }
        public int FanJi { get; set; }
        public int ShanDuo { get; set; }
        public int MoGong { get; set; }
        public int KangMo { get; set; }
        public int ShiHua { get; set; }
        public int Du { get; set; }
        public int Zui { get; set; }
        public int HunShui { get; set; }
        public int YiWang { get; set; }
        public int HunLuan { get; set; }
        [ForeignKey("Stall"),Column(Order = 0)]
        public int StallX { get; set; }
        [ForeignKey("Stall"),Column(Order = 1)]
        public int StallY { get; set; }
        public Stall Stall { get; set; }

    }
}
