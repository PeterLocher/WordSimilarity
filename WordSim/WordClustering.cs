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
            var wordClusters = FindWordClusters(words, 4, 2, 5);
            var serializer = new Newtonsoft.Json.JsonSerializer();
            using (StreamWriter streamWriter = new StreamWriter("out/word_clusters.json"))
            using (JsonWriter writer = new JsonTextWriter(streamWriter))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, wordClusters.Dictionary);
            }
        }
        
        private static string[] LoadWords(string path)
        {
            var westEuropeEncoding = Encoding.GetEncoding(28591);
            var lexemes = File.ReadAllLines(path, westEuropeEncoding);
            var words = lexemes.Where(lexeme => lexeme[0] != '-').ToArray();
            return words;
        }

        
        /*
         * Computes a dictionary of strings in 'words' longer than minWordLength mapped to a list of their
         * similar words, when there are at least minClusterSize of them.
         * Similar words are words within a StringMetric Distance limited by the internalClusterDistance parameter.
         * Returns the dictionary wrapped by the WordCluster class.
         */
        private static WordCluster FindWordClusters(string[] words, int minWordLength, int internalClusterDistance, int minClusterSize)
        {
            var wordClusters = new Dictionary<string, List<string>>();

            foreach (var word in words)
            {
                if (word.Length < minWordLength || wordClusters.ContainsKey(word)) continue;
                var wordCluster = new List<string>();
                foreach (var word2 in words)
                {
                    if (Distance(word, word2) < internalClusterDistance)
                    {
                        wordCluster.Add(word2);
                    }
                }

                wordClusters[word] = wordCluster;
            }

            wordClusters = wordClusters.Where(pair => pair.Value.Count > minClusterSize)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            return new WordCluster(wordClusters);
        }
    }
}