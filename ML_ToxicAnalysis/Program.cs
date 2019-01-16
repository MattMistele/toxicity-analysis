using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Transforms.Text;

namespace ML_ToxicAnalysis
{
    class Program
    {
        static TextLoader _textLoader;

        static void Main(string[] args)
        {
            // Create a new MLContext to do the ML stuff
            MLContext mlContext = new MLContext(seed: 0);

            // Load in the data into our _textLoader
            _textLoader = mlContext.Data.TextReader(new TextLoader.Arguments()
            {
                Separator = "tab",
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("Label", DataKind.Bool, 0),
                    new TextLoader.Column("SentimentText", DataKind.Text, 1)
                }
            });

            // Run the training method
            //var model = Train(mlContext, Constants._trainDataPath);

            // Now, see how the model did
            //Evaluate(mlContext, model);

            // Predict some data using our model
            //Predict(mlContext, model);

            // Predict some data using a model loaded from a file
            List<SentimentData> comments = CommentLoader.LoadYoutubeComments(Constants._WSJ_collection_Path);
            PredictWithModelLoadedFromFile(mlContext, comments);
        }

        // The magic happens here
        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            // Load the data. 
            // The DataView object is similar to a list
            IDataView dataView = _textLoader.Read(dataPath);

            // Transform our data into what ML aglorithms can recognize: a numeric vector
            // Basically it'll be a pipeline for our data
            var pipeline = mlContext.Transforms.Text.FeaturizeText("SentimentText", "Features")
                // Use the 'FastTree' ML algoritm
                .Append(mlContext.BinaryClassification.Trainers.FastTree(numLeaves: 50, numTrees: 50, minDatapointsInLeaves: 20));

            // Train the model
            Console.WriteLine("=============== Create and Train the Model ===============");

            // .Fit() trains the pipeline and returns a model to be used for predictions. Easy as that.
            var model = pipeline.Fit(dataView);

            Console.WriteLine("=============== End of training ===============");
            Console.WriteLine();

            // Return + save our model for future predictions
            return model;
        }

        // Method to evaluate our ML model for quality assurance
        public static void Evaluate(MLContext mlContext, ITransformer model)
        {
            // Load the testing data
            IDataView dataView = _textLoader.Read(Constants._testDataPath);

            // Evaluate the model
            Console.WriteLine("=============== Evaluating Model accuracy with Test data===============");
            var predictions = model.Transform(dataView);

            // .Evaluate() computes the quality metrics
            var metrics = mlContext.BinaryClassification.Evaluate(predictions, "Label");

            // Print the results
            Console.WriteLine();
            Console.WriteLine("Model quality metrics evaluation");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.Auc:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
            Console.WriteLine("=============== End of model evaluation ===============");

            // Save the model as a .zip file
            SaveModelAsFile(mlContext, model);
        }

        // Method to predict a single comment
        private static void Predict(MLContext mlContext, ITransformer model)
        {
            // Create our predictionFunction
            var predictionFunction = model.MakePredictionFunction<SentimentData, SentimentPrediction>(mlContext);

            // Sample comment for the model to make a prediction on
            // TODO: Make this a randomly generated YouTube comment or something
            SentimentData sampleStatement = new SentimentData
            {
                SentimentText = "This is a very rude movie"
            };

            // Predict! Featurization and pipeline sync is taken care for us
            var result = predictionFunction.Predict(sampleStatement);

            // Print out the result
            Console.WriteLine();
            ToxicityDataAnalysis.PrintResult((sampleStatement, result));
        }

        public static void PredictWithModelLoadedFromFile(MLContext mlContext, List<SentimentData> comments)
        {
            // Load the model
            ITransformer loadedModel;
            using (var stream = new FileStream(Constants._modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = mlContext.Model.Load(stream);
            }

            // Create prediction engine
            var sentimentStreamingDataView = mlContext.CreateStreamingDataView(comments);
            var predictions = loadedModel.Transform(sentimentStreamingDataView);

            // Use the model to predict whether comment data is toxic (1) or nice (0).
            var predictedResults = predictions.AsEnumerable<SentimentPrediction>(mlContext, reuseRowObject: false);

            Console.WriteLine();

            // combine the sentiments and predictions together
            var sentimentsAndPredictions = comments.Zip(predictedResults, (sentiment, prediction) => (sentiment, prediction));

           // ToxicityDataAnalysis.PrintResults(sentimentsAndPredictions);
            ToxicityDataAnalysis.DataAnalysis(sentimentsAndPredictions);
        }

        // Save the model as a .zip file to be reused in the future
        private static void SaveModelAsFile(MLContext mlContext, ITransformer model)
        {
            using (var fs = new FileStream(Constants._modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                mlContext.Model.Save(model, fs);

            // Display where the file is written to
            Console.WriteLine("The model is saved to {0}", Constants._modelPath);
        }
    }
}
