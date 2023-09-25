using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WordSim
{
    public class WordClustering
    {
        private delegate int StringMetric(string w1, string w2);

        private static readonly StringMetric Distance = StringMetrics.LevenshteinDistance;

        public static void ClusterDuolingoWords()
        {
            var words = LoadWords("in/words.csv");
            var wordClusters = FindWordClusters(words);
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter("out/word_clusters.json"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, wordClusters);
            }
        }
        
        private static string[] LoadWords(string path)
        {
            var westEuropeEncoding = Encoding.GetEncoding(28591);
            var lexemes = File.ReadAllLines(path, westEuropeEncoding);
            var words = lexemes.Where(lexeme => lexeme[0] != '-').ToArray();
            return words;
        }

        private static WordCluster FindWordClusters(string[] words)
        {
            var wordClusters = new Dictionary<string, List<string>>();

            foreach (var word in words)
            {
                if (word.Length < 4 || wordClusters.ContainsKey(word)) continue;
                var wordCluster = new List<string>();
                foreach (var word2 in words)
                {
                    if (Distance(word, word2) < 2)
                    {
                        wordCluster.Add(word2);
                    }
                }

                wordClusters[word] = wordCluster;
            }

            wordClusters = wordClusters.Where(pair => pair.Value.Count > 5)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            return new WordCluster(wordClusters);
        }
    }
}