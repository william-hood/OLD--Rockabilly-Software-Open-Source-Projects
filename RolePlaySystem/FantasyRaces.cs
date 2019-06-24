using System;
namespace RolePlaySystem
{
    public sealed class Human : CharacterRace
    {
        #region Singleton
        private static readonly Human instance = new Human();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Human()
        {
        }

        private Human()
        {
        }

        public static Human Singleton
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

    public sealed class Dwarf : CharacterRace
    {
        #region Singleton
        private static readonly Dwarf instance = new Dwarf();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Dwarf()
        {
        }

        private Dwarf()
        {
        }

        public static Dwarf Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => -2;
        public override int StrengthModifier => 3;
        public override int IntellectModifier => 0;
        public override int DexterityModifier => 2;
        public override int CharismaModifier => -1;
    }

    public sealed class Elf : CharacterRace
    {
        #region Singleton
        private static readonly Elf instance = new Elf();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Elf()
        {
        }

        private Elf()
        {
        }

        public static Elf Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 0;
        public override int StrengthModifier => -2;
        public override int IntellectModifier => 4;
        public override int DexterityModifier => 4;
        public override int CharismaModifier => 5;
    }

    public sealed class Orc : CharacterRace
    {
        #region Singleton
        private static readonly Orc instance = new Orc();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Orc()
        {
        }

        private Orc()
        {
        }

        public static Orc Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 0;
        public override int StrengthModifier => 3;
        public override int IntellectModifier => -2;
        public override int DexterityModifier => 0;
        public override int CharismaModifier => -15;
    }

    public sealed class Nosferatu : CharacterRace
    {
        #region Singleton
        private static readonly Nosferatu instance = new Nosferatu();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nosferatu()
        {
        }

        private Nosferatu()
        {
        }

        public static Nosferatu Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 20;
        public override int StrengthModifier => 30;
        public override int IntellectModifier => 20;
        public override int DexterityModifier => 5;
        public override int CharismaModifier => 15;
    }

    public sealed class Cavedweller : CharacterRace
    {
        #region Singleton
        private static readonly Cavedweller instance = new Cavedweller();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Cavedweller()
        {
        }

        private Cavedweller()
        {
        }

        public static Cavedweller Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 15;
        public override int StrengthModifier => 25;
        public override int IntellectModifier => -15;
        public override int DexterityModifier => 0;
        public override int CharismaModifier => -10;
    }
}
