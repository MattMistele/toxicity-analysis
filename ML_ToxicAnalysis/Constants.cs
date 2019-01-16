using System;
using System.IO;

namespace ML_ToxicAnalysis
{
    public static class Constants
    {
        public static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "Training_Data", "wikipedia-detox-250-line-data.tsv");
        public static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "Training_Data", "wikipedia-detox-250-line-test.tsv");
        public static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        public static readonly string _sampleCommentsPath = Path.Combine(Environment.CurrentDirectory, "Data", "comments.txt");

        public static readonly string _YT_Rewind_Path = Path.Combine(Environment.CurrentDirectory, "Data", "YT_comments", "rewind.json");
        public static readonly string _WSJ_collection_Path = Path.Combine(Environment.CurrentDirectory, "Data", "YT_comments", "WSJ_collection.json");
        public static readonly string _Vsauce_collection_Path = Path.Combine(Environment.CurrentDirectory, "Data", "YT_comments", "Vsauce_collection.json");
        public static readonly string _Khan_Academy_collection_Path = Path.Combine(Environment.CurrentDirectory, "Data", "YT_comments", "Khan_Academy_collection.json");
    }
}
