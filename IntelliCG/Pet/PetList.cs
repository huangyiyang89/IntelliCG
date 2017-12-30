using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliCG.Pet
{
    public class PetList:Base
    {
        private readonly List<Pet> _pets;

        public PetList(int hwnd) : base(hwnd)
        {
            _pets=new List<Pet>();
            for (var i = 0; i < 5; i++)
            {
                _pets.Add(new Pet(hwnd,i));
            }
        }

        public Pet this[int index] => _pets[index];

        public Pet CombatStatePet
        {
            get { return _pets.Find(p => p.State == PetState.Combat); }
        }
    }
}
