using System;
namespace RolePlaySystem
{
    public abstract class CharacterModifier
    {
        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public abstract int HitPointModifier { get; }
        public abstract int StrengthModifier { get; }
        public abstract int IntellectModifier { get; }
        public abstract int DexterityModifier { get; }
        public abstract int CharismaModifier { get; }
    }

    public abstract class CharacterGender : CharacterModifier { }
    public abstract class CharacterRace : CharacterModifier { }
    public abstract class CharacterClass : CharacterModifier { }
}

