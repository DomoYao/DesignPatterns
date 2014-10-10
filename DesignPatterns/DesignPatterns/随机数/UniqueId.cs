using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DesignPatterns.UniqueId
{
    public static class UniqueId
    {
        private static long _sequence = DateTime.UtcNow.Ticks;

        public static long Generate()
        {
            return Interlocked.Increment(ref _sequence);
        }

        public static long Sequence
        {
            get
            {
                return _sequence;
            }
        }
    }
}
