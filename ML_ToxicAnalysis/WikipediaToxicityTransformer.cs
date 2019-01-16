using System;
using System.Collections.Generic;
using System.IO;

namespace ML_ToxicAnalysis
{
    // Created by Nick Bitounis: https://gist.github.com/nickntg/1ce67727d3047146ffe0f8192aba6177

    // Transforms the Wikipedia toxicity dataset (https://meta.wikimedia.org/wiki/Research:Detox/Data_Release)
    // into a format our ML aglorithm can process

    public class WikipediaToxicityTransformer
    {
        public void CreateToxicityFilesFromWikipediaSets(string trainDataPath, string testDataPath)
        {

            var classificationMap = new Dictionary<string, string>();

            var currentId = string.Empty;
            var ones = 0;
            var zeroes = 0;

            ReadFileAndProcessContents("toxicity_annotations.tsv", spl =>
            {
                var id = spl[0];
                if (id != currentId)
                {
                    var total = ones + zeroes;
                    if (total > 0)
                    {
                        // The dataset contains several opinions per entry listed. We count the
                        // opinion of the authors and require that at least 90% of them should agree
                        // with a classification in order to use it.
                        if (100 * ones / total >= 90)
                        {
                            classificationMap.Add(currentId, "1");
                        }
                        else if (100 * zeroes / total >= 90)
                        {
                            classificationMap.Add(currentId, "0");
                        }
                    }
                    ones = 0;
                    zeroes = 0;
                    currentId = id;
                }

                switch (spl[2])
                {
                    case "0":
                        zeroes++;
                        break;
                    case "1":
                        ones++;
                        break;
                }
            });

            var commentMap = new Dictionary<string, Comment>();

            ReadFileAndProcessContents("toxicity_annotated_comments.tsv", spl =>
            {
                var id = spl[0];
                if (classificationMap.ContainsKey(id))
                {
                    commentMap.Add(id, new Comment { Id = id, Toxic = classificationMap[id], Dataset = spl[6], Text = spl[1] });
                }
            });

            using (var train = new StreamWriter(trainDataPath))
            using (var test = new StreamWriter(testDataPath))
            {
                train.WriteLine("Sentiment	SentimentText");
                test.WriteLine("Sentiment	SentimentText");
                foreach (var comment in commentMap.Values)
                {
                    StreamWriter sw = null;
                    switch (comment.Dataset)
                    {
                        case "train":
                            sw = train;
                            break;
                        case "test":
                            sw = test;
                            break;
                    }
                    sw?.WriteLine($"{comment.Toxic}\t{comment.Text}");
                }
            }
        }

        private void ReadFileAndProcessContents(string fileName, Action<string[]> action)
        {
            using (var fs = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Data", fileName)))
            {
                fs.ReadLine();
                while (!fs.EndOfStream)
                {
                    var line = fs.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    var spl = line.Split('\t');
                    action(spl);
                }
            }
        }
    }

    public class Comment
    {
        public string Id { get; set; }
        public string Toxic { get; set; }
        public string Dataset { get; set; }
        public string Text { get; set; }
    }
}
