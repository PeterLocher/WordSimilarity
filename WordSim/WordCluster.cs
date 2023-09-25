using System.Collections.Generic;

namespace WordSim
{
    public class WordCluster
    {
        private Dictionary<string, List<string>> _dictionary;

        public WordCluster(Dictionary<string, List<string>> dictionary)
        {
            _dictionary = dictionary;
        }
    }
}