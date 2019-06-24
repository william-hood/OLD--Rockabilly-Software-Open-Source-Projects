using System;
namespace RolePlaySystem
{
    public abstract class Character
    {
        CharacterClass Class;
        CharacterRace Race;
        CharacterGender Gender;

        private int baseHitPoints;
        public int MaxHitPoints
        {
            get
            {
                return baseHitPoints + Class.HitPointModifier + Race.HitPointModifier + Gender.HitPointModifier;
            }
        }

        public int HitPoints;

        private int baseStrength;
        public int Strength
        {
            get
            {
                int result = baseStrength + Class.StrengthModifier + Race.StrengthModifier + Gender.StrengthModifier;
                if (result > 1) return result;
                return 1;
            }
        }

        private int baseIntellect;
        public int Intellect
        {
            get
            {
                int result = baseIntellect + Class.IntellectModifier + Race.IntellectModifier + Gender.IntellectModifier;
                if (result > 1) return result;
                return 1;
            }
        }

        private int baseDexterity;
        public int Dexterity
        {
            get
            {
                int result = baseDexterity + Class.DexterityModifier + Race.DexterityModifier + Gender.DexterityModifier;
                if (result > 1) return result;
                return 1;
            }
        }

        private int baseCharisma;
        public int Charisma
        {
            get
            {
                int result = baseCharisma + Class.CharismaModifier + Race.CharismaModifier + Gender.CharismaModifier;
                if (result > 1) return result;
                return 1;
            }
        }

        private Dice challengeDie = new Dice(100);
        private Outcome checkAttribute(int effectiveValue)
        {
            int result = challengeDie.Roll;

            if (result == 1)
            {
                if (result <= effectiveValue)
                {
                    return Outcome.CriticalSuccess;
                }
            }

            if (result == 100)
            {
                if (result > effectiveValue)
                {
                    return Outcome.CriticalFailure;
                }
            }

            if (result > effectiveValue)
            {
                return Outcome.Failure;
            }

            return Outcome.Success;
        }

        public Outcome ConstitutionCheck
        {
            get
            {
                return checkAttribute(HitPoints);
            }
        }

        // Hitting armor might be treated as a contest between attacker's strength and the victim's armor.
        // Armor may prevent a strike from going through and/or reduce the amount that gets through.
        public Outcome StrengthCheck
        {
            get
            {
                return checkAttribute(Strength);
            }
        }

        public Outcome IntellectCheck
        {
            get
            {
                return checkAttribute(Intellect);
            }
        }

        public Outcome DexterityCheck
        {
            get
            {
                return checkAttribute(Dexterity);
            }
        }
    }
}
