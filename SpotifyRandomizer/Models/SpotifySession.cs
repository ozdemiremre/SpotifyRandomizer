using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyRandomizer.Models
{
    public static class SpotifySession
    {
        public static ISpotifyHandler ActiveSession { get; private set; }

        public static void Initialize()
        {
#if DEBUG_WTEST
            ActiveSession = new TestSpotifyHandler();
#else
            ActiveSession = new DefaultSpotifyHandler();
#endif
        }

    }
}
