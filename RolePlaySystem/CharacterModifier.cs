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
    }
}
