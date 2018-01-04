using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.MemoryHelper;

namespace IntelliCG.Item
{
    public class ItemList:Base
    {
        public ItemList(Memo memo) : base(memo)
        {
            _items = new List<Item>();
            for (var i = 0; i < 20; i++)
            {
                _items.Add(new Item(memo,i));
            }
            
        }

        private readonly List<Item> _items;

        public Item this[int index] => _items[index];


        public List<Item> Drugs
        {
            get { return _items.FindAll(i => i.TypeName == "药"); }
           
        }

        public List<Item> Foods
        {
            get { return _items.FindAll(i => i.TypeName == "料理"); }
        }

    }
}
