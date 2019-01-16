using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML_ToxicAnalysis
{
    public class ToxicityDataAnalysis
    {
        // This method:
        // 1) Calculates the precentage of toxic comments in a dataset
        // 2) Prints out all of the toxic comments in ascending order of probability
        public static void DataAnalysis(IEnumerable<(SentimentData sentiment, SentimentPrediction prediction)> sentimentsAndPredictions)
        {
            double totalComments = 0;
            double toxicComments = 0;

            // Sort the dataset by probability
            sentimentsAndPredictions = sentimentsAndPredictions.OrderBy(comment => comment.prediction.Probablility);

            foreach (var item in sentimentsAndPredictions)
            {
                bool prediction = item.prediction.Prediction;
                totalComments++;

                if (prediction)
                {
                    PrintResult(item);
                    toxicComments++;
                }
            }
            double precentToxic = (toxicComments / totalComments) * 100;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(precentToxic.ToString("0.##\\%"));
            Console.ForegroundColor = ConsoleColor.White; Console.Write(" of WSJ Youtube comments are");
            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(" toxic");
            Console.ResetColor(); Console.WriteLine();
        }

        // This method:
        // 1) Prints out each comment,
        // 2) If it's toxic or non-toxic,
        // 3) The probability as a precentage from 0 - 100. Where 0 is certainly Non-Toxic and 100 is certainly Toxic  
        public static void PrintResults(IEnumerable<(SentimentData sentiment, SentimentPrediction prediction)> sentimentsAndPredictions)
        {
            foreach (var item in sentimentsAndPredictions)
            {
                PrintResult(item);
            }
            Console.WriteLine();
        }

        // This method will print out a comment and
        // 1) It's prediction of being toxic or non-toxic,
        // 2) The probability of the prediction as a precentage from 0 - 100. 
        //    Where 0 is certainly Non-Toxic and 100 is certainly Toxic  
        public static void PrintResult((SentimentData sentiment, SentimentPrediction prediction) item)
        {
            string precentage = (item.prediction.Probablility * 100).ToString("0.##\\%");
            bool prediction = item.prediction.Prediction;
            string text = item.sentiment.SentimentText;

            Console.ForegroundColor = ConsoleColor.Blue; Console.Write("C: ");
            Console.ResetColor(); Console.Write(text);
            Console.CursorLeft = Console.BufferWidth - 20;

            if (prediction)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(Convert.ToBoolean(prediction) ? "Toxic  " : "Not Toxic  ");
            Console.ResetColor();

            Console.WriteLine(precentage);
        }
    }
}
