using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IStatsProvider
    {
        public IEnumerable<float> GetModifiers(Stats stat);
    }
}
