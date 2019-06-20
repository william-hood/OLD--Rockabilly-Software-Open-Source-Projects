using System;
namespace RolePlaySystem
{
    public enum Outcome
    {
        CriticalFailure = 0,
        Failure = 1,
        Success = 2,
        CriticalSuccess = 3
    }

    public static class OutcomeExtensions
    {
        public static bool Succeeds(this Outcome value)
        {
            return value > Outcome.Failure;
        }

        public static bool Fails(this Outcome value)
        {
            return value < Outcome.Success;
        }
    }
}
