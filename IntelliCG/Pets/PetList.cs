
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using MemoLib;

namespace IntelliCG.Pets
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
            get { return _pets.Find(p => p.State == PetState.Combat); }
        }

        public int Count
        {
            get
            {
                return _pets.FindAll(p => p.Exist).Count;
            }
        }

        public List<Pet> Posters
        {
            get
            {
                var pets=new List<Pet>();
                foreach (var pet in _pets)
                {
                    if (pet.IsPoster)
                    {
                        pets.Add(pet);
                    }
                }
                return pets;
            }
        }

    }

    
}
