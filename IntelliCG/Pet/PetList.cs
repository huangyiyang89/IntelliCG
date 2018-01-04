using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelliCG.MemoryHelper;

namespace IntelliCG.Pet
{
    public class PetList:Base
    {
        private readonly List<Pet> _pets;

        public PetList(Memo memo) : base(memo)
        {
            _pets=new List<Pet>();
            for (var i = 0; i < 5; i++)
            {
                _pets.Add(new Pet(memo,i));
            }
        }

        public Pet this[int index] => _pets[index];
        public Pet this[string name] => _pets.Find(p=>p.Name.Contains(name));

        public Pet CombatStatePet
        {
            get { return _pets.FirstOrDefault(p => p.State == PetState.Combat); }
        }
    }
}
