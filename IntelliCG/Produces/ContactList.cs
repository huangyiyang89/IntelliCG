using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoLib;

namespace IntelliCG.Produces
{
    public class ContactList:Base
    {
        //S_K 3 1 9 123123 0     S_K 3 0 a 123123 0     名片第几个  第几个宠物  第几个物品   
        private readonly List<Contact> _contacts;
        public ContactList(Memo memo) : base(memo)
        {
            _contacts = new List<Contact>();
            for (var i = 0; i < 50; i++)
            {
                var contact=new Contact(Memo,i);
                 _contacts.Add(contact);
                
            }
        }

        public Contact FindExact(string name)
        {
            var contact=_contacts.Find(c => c.Name==name);
            return contact ?? _contacts.Find(c => c.Name.Contains(name));
        }
    }
}
