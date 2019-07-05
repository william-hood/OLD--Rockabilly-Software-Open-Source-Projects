using System;
using System.Threading;
using Rockabilly.Common;

namespace Rockabilly.CoarseGrind.PerformanceTesting
{
    public abstract class SequenceDelay
    {
        public abstract void TakeEffect();
    }

    public class RandomSequenceDelay : SequenceDelay
    {
        private TimeSpan min = default(TimeSpan);
        private TimeSpan max = default(TimeSpan);

        public RandomSequenceDelay(TimeSpan minimumDelay, TimeSpan maximumDelay)
        {
            min = minimumDelay;
            max = maximumDelay;
        }

        public override void TakeEffect()
        {
            Thread.Sleep(RandomUtilities.Random.Next(min.Milliseconds, max.Milliseconds));
        }
    }

    public class FixedDelay : SequenceDelay
    {
        private TimeSpan Delay = default(TimeSpan);

        public FixedDelay(TimeSpan delay)
        {
            Delay = delay;
        }

        public override void TakeEffect()
        {
            Thread.Sleep(Delay);
        }
    }
}
