using System.Collections.Generic;

namespace WordSim
{
    public class WordCluster
    {
        public Dictionary<string, List<string>> Dictionary { get; }

        public WordCluster(Dictionary<string, List<string>> dictionary)
        {
            Dictionary = dictionary;
        }
    }
}