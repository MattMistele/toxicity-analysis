using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ML_ToxicAnalysis
{
    public class CommentLoader
    {
        // Parse and load Youtube Comments from JSON
        public static List<SentimentData> LoadYoutubeComments(string path)
        {
            List<SentimentData> result = new List<SentimentData>();

            foreach (string line in File.ReadLines(path))
            {
                JObject jObject = JObject.Parse(line);

                result.Add(new SentimentData
                {
                    SentimentText = jObject["text"].ToString()
                });
            }

            return result;
        }

        // Parse and load comments seperated by line from a file
        private static List<SentimentData> LoadCommentsFromFile(string path)
        {
            List<SentimentData> result = new List<SentimentData>();

            foreach (string line in File.ReadLines(path))
            {
                result.Add(new SentimentData
                {
                    SentimentText = line
                });
            }

            return result;
        }
    }
}
