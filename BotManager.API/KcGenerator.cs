using System;

namespace BotManager.Api
{
    public static class KcGenerator
    {

        static Random _rng = new Random();

        public static int GetRandomKcCount(int max)
        {
            int min = 1;

            if (max > 17)
            {
                max = 17;
            }

            if (max < 5)
            {
                min += 2;
            }

            if (min > max)
            {
                min = max;
            }

            return _rng.Next(min, max);
        }
    }
}
