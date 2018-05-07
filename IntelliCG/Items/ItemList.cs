using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using IntelliCG.Extensions;
using MemoLib;

namespace IntelliCG.Items
{
    public class ItemList:CgBase
    {
        public ItemList(CrossGate cg) : base(cg)
        {
            _items = new List<Item>();
            for (var i = 0; i < 47; i++)
            {
                _items.Add(new Item(Memo,i));
            }

            
        }

        private readonly List<Item> _items;

        public Item this[int index] => _items[index];

        public List<Item> All
        {
            get { return _items.FindAll(i => i.Exist); }
        }

        

        public List<Item> Drugs
        {
            get { return _items.FindAll(i =>i.Exist&& i.TypeName == "药"); }
           
        }

        public List<Item> Foods
        {
            get { return _items.FindAll(i =>i.Exist&& i.TypeName == "料理"); }
        }

        public int FindExactIndex(string name,int count)
        {
            return _items.FindIndex(i =>i.Name==name && i.Count >= count)+1;
        }

        public Item FindExact(string name)
        {
            var item=_items.Find(i => i.Name==name);
            return item ?? _items.Find(i => i.Name.Contains(name));
        }

        public Item FindExactFull(string name)
        {
            var item=_items.Find(i => i.Name==name&&i.IsFull);
            return item ?? _items.Find(i => i.Name.Contains(name)&&i.IsFull);
        }

        public async Task Arrange()
        {
            for (var i = 0; i < 47; i++)
            {
                for (var j = i+1; j < 47; j++)
                {
                    var item1 = _items[i];
                    if (!item1.Exist)
                    {
                        break;
                    }
                    var item2 = _items[j];
                    if (item1.IsFull)
                    {
                        break;
                    }
                    if (!item2.Exist)
                    {
                        continue;
                    }
                    if (item2.Name != item1.Name)//不是同种物品
                    {
                        continue;
                    }

                    if (item2.IsFull)
                    {
                        continue;
                    }
                    if (item2.Count + item1.Count >= item1.MaxCount)
                    {
                        await Put(item2, item1);
                        await Task.Delay(300);
                        break;
                    }
                    await Put(item2, item1);
                    await Task.Delay(300);
                }
            }

            await Task.Delay(500);
        }

        //a放到b
        public async Task Put(Item a,Item b)
        {
            var str = @"oNg " + a.Index8.To62String() + " " + b.Index8.To62String() + " -1 ";
            await Cg.Actions.DecodeSend(str);
        }

        public Item GetOneGem()
        {
            return _items.Find(i =>i.Exist&&i.TypeName == "宝石");
        }


        public void Test()
        {
            foreach (var item in _items)
            {
                if (item.Exist)
                {
                    Console.WriteLine($@"{ item.Index8} {item.Name} {item.Count}");
                }
            }
        }
       
    }
}
