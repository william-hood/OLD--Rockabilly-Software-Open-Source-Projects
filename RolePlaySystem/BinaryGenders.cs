using System;
namespace RolePlaySystem
{
    public sealed class Female : CharacterGender
    {
        #region Singleton
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
        #endregion

        public override int HitPointModifier => -1;
        public override int StrengthModifier => -1;
        public override int IntellectModifier => 1;
        public override int DexterityModifier => 0;
        public override int CharismaModifier => 1;
    }

    public sealed class Male : CharacterGender
    {
        #region Singleton
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
        #endregion

        public override int HitPointModifier => 0;
        public override int StrengthModifier => 1;
        public override int IntellectModifier => 0;
        public override int DexterityModifier => 0;
        public override int CharismaModifier => 0;
    }
}
