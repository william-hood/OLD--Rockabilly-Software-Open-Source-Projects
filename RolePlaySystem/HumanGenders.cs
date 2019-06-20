using System;
namespace RolePlaySystem
{
    public sealed class Female : CharacterGender
    {
        private static readonly Female instance = new Female();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Female()
        {
        }

        private Female()
        {
        }

        public static Female Singleton
        {
            get
            {
                return instance;
            }
        }

        public override int HitPointModifier => -1;

        public override int StrengthModifier => -1;

        public override int IntellectModifier => 1;

        public override int DexterityModifier => 0;
    }

    public sealed class Male : CharacterGender
    {
        private static readonly Male instance = new Male();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Male()
        {
        }

        private Male()
        {
        }

        public static Male Singleton
        {
            get
            {
                return instance;
            }
        }

        public override int HitPointModifier => 0;

        public override int StrengthModifier => 1;

        public override int IntellectModifier => 0;

        public override int DexterityModifier => 0;
    }
}
