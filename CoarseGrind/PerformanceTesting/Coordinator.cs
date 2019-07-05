using System;
using System.Threading;

namespace Rockabilly.CoarseGrind.PerformanceTesting
{
    public static class Coordinator
    {
        internal static CountdownEvent StopSignal = new CountdownEvent(1);
        //internal static CountdownEvent StopAcknowledgement; // To be set later

        private static int expectedAcknowledgements = 0;
        public static void DeclareProcessStart()
        {
            Interlocked.Add(ref expectedAcknowledgements, 1);
        }
    }
}
