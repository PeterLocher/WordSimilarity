using System;

namespace WordSim
{
    public static class StringMetrics
    {

        public static bool Log = false;
        
        
        /*
         * Computes the Levenshtein distance between two strings as defined in figure 3,
         * https://hcommons.org/deposits/objects/hc:46164/datastreams/CONTENT/content
         * using a dynamic O(w1 x w2) time and space algorithm
         */
        public static int LevenshteinDistance2DArray(string word1, string word2)
        {
            int n = word1.Length, m = word2.Length;
            var distances = new int[n + 2, m + 2];
            
            for (var i = 0; i <= n; i++)
            {
                distances[i, 0] = i;
            }
            
            for (var j = 0; j <= m; j++)
            {
                distances[0, j] = j;
            }

            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= m; j++)
                {
                    var costDelete = distances[i - 1, j] + 1;
                    var costInsert = distances[i, j - 1] + 1;
                    // We let eq represent a substitution cost of 0 when the letters are equal
                    var eq = word1[i - 1] == word2[j - 1] ? 0 : 1;
                    var costSubstitute = distances[i - 1, j - 1] + eq;
                    distances[i, j] = Min(costDelete, costInsert, costSubstitute);
                }
            }

            if (Log) PrintLevArray(distances, word1, word2);                
            
            return distances[n, m];
        }
        
        public static int LevenshteinDistance(string word1, string word2)
        {
            int n = word1.Length, m = word2.Length;
            var distances = new int[m + 2];
            
            for (var i = 0; i <= m; i++)
            {
                distances[i] = i;
            }

            for (var i = 1; i <= n; i++)
            {
                var nextDistances = new int[m + 2];
                nextDistances[0] = i;
                for (var j = 1; j <= m; j++)
                {
                    var costDelete = distances[j] + 1;
                    var costInsert = nextDistances[j - 1] + 1;
                    // We let eq represent a substitution cost of 0 when the letters are equal
                    var eq = word1[i - 1] == word2[j - 1] ? 0 : 1;
                    var costSubstitute = distances[j - 1] + eq;
                    nextDistances[j] = Min(costDelete, costInsert, costSubstitute);
                }
                
                distances = nextDistances;
                
                if (Log)
                {
                    for (var j = 0; j <= m; j++)
                    {
                        Console.Write(distances[j] + " ");
                    }
                    Console.WriteLine();
                }
            }
            
            return distances[m];
        }

        private static int Min(int x, int y, int z)
        {
            return Math.Min(x, Math.Min(y, z));
        }
        
        private static void PrintLevArray(int[,] array, string word1, string word2)
        {
            Console.Write("\\   ");
            
            foreach (var c in word2)
            {
                Console.Write(c + " ");
            }
            Console.WriteLine();
            for (var i = 0; i < array.GetUpperBound(0); i++)
            {
                char c = i > 0 ? word1[i - 1] : ' ';
                Console.Write(c + " ");
                
                for (var j = 0; j < array.GetUpperBound(1); j++)
                {
                    Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}