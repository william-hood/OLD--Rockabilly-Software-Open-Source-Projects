using System;
using System.Threading;
namespace Rockabilly.CoarseGrind.PerformanceTesting
{
    public abstract class Sequence
    {
        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public SequenceDelay DelayBetweenSequences = new FixedDelay(new TimeSpan(0, 0, 0));

        public abstract void CarryOutSequence();

        private void mainLoop()
        {
            while (!Coordinator.StopSignal.IsSet)
            {
                if (!Coordinator.StopSignal.IsSet)
                {
                    DelayBetweenSequences.TakeEffect();
                }

                if (!Coordinator.StopSignal.IsSet)
                {
                    CarryOutSequence();
                }
            }

            // Coordinator.StopAcknowledgement.Signal();// Defined in Coordinator. Not yet implemented.
        }
    }
}
