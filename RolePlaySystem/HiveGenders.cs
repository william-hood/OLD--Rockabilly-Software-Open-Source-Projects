using System;
namespace RolePlaySystem
{
    public sealed class HiveWorker : CharacterGender
    {
        #region Singleton
        private static readonly HiveDrone instance = new HiveWorker();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static HiveWorker()
        {
        }

        private HiveWorker()
        {
        }

        public static HiveWorker Singleton
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
    public sealed class HiveDrone : CharacterGender
    {
        #region Singleton
        private static readonly HiveDrone instance = new HiveDrone();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static HiveDrone()
        {
        }

        private HiveDrone()
        {
        }

        public static HiveDrone Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => -2;
        public override int StrengthModifier => -2;
        public override int IntellectModifier => -1;
        public override int DexterityModifier => -1;
        public override int CharismaModifier => -1;
    }

    public sealed class HiveRoyal : CharacterGender
    {
        #region Singleton
        private static readonly HiveRoyal instance = new HiveRoyal();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static HiveRoyal()
        {
        }

        private HiveRoyal()
        {
        }

        public static HiveRoyal Singleton
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public override int HitPointModifier => 5;
        public override int StrengthModifier => 3;
        public override int IntellectModifier => 3;
        public override int DexterityModifier => 4;
        public override int CharismaModifier => 5;
    }
}
