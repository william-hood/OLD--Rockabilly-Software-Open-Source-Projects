using System;
namespace RolePlaySystem
{
    public sealed class Commoner : CharacterClass
    {
        #region Singleton
        private static readonly Commoner instance = new Commoner();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Commoner()
        {
        }

        private Commoner()
        {
        }

        public static Commoner Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 0;
        public override int StrengthModifier => 0;
        public override int IntellectModifier => 0;
        public override int DexterityModifier => 0;
        public override int CharismaModifier => 0;
    }

    public sealed class Wizard : CharacterClass
    {
        #region Singleton
        private static readonly Wizard instance = new Wizard();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Wizard()
        {
        }

        private Wizard()
        {
        }

        public static Wizard Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => -5;
        public override int StrengthModifier => -5;
        public override int IntellectModifier => 10;
        public override int DexterityModifier => 0;
        public override int CharismaModifier => 0;
    }

    public sealed class Cleric : CharacterClass
    {
        #region Singleton
        private static readonly Cleric instance = new Cleric();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Cleric()
        {
        }

        private Cleric()
        {
        }

        public static Cleric Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => -1;
        public override int StrengthModifier => -1;
        public override int IntellectModifier => 5;
        public override int DexterityModifier => 0;
        public override int CharismaModifier => 2;
    }

    public sealed class Soldier : CharacterClass
    {
        #region Singleton
        private static readonly Soldier instance = new Soldier();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Soldier()
        {
        }

        private Soldier()
        {
        }

        public static Soldier Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 5;
        public override int StrengthModifier => 5;
        public override int IntellectModifier => 0;
        public override int DexterityModifier => 5;
        public override int CharismaModifier => -1;
    }

    public sealed class Barbarian : CharacterClass
    {
        #region Singleton
        private static readonly Barbarian instance = new Barbarian();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Barbarian()
        {
        }

        private Barbarian()
        {
        }

        public static Barbarian Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 10;
        public override int StrengthModifier => 10;
        public override int IntellectModifier => -5;
        public override int DexterityModifier => 5;
        public override int CharismaModifier => -3;
    }

    public sealed class Marksman : CharacterClass
    {
        #region Singleton
        private static readonly Marksman instance = new Marksman();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Marksman()
        {
        }

        private Marksman()
        {
        }

        public static Marksman Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 0;
        public override int StrengthModifier => 5;
        public override int IntellectModifier => 5;
        public override int DexterityModifier => 15;
        public override int CharismaModifier => 0;
    }

    public sealed class Mastermind : CharacterClass
    {
        #region Singleton
        private static readonly Mastermind instance = new Mastermind();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Mastermind()
        {
        }

        private Mastermind()
        {
        }

        public static Mastermind Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 5;
        public override int StrengthModifier => 10;
        public override int IntellectModifier => 30;
        public override int DexterityModifier => 10;
        public override int CharismaModifier => 15;
    }
}
