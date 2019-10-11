using Microsoft.ML;
using System;
using TaxiFarePredictionAppML.Model;

namespace TaxiFarePredictionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var context = new MLContext(seed: 0);
                var modelFile = AppDomain.CurrentDomain.BaseDirectory + "MLModel.zip";
                var model = context.Model.Load(filePath: modelFile, out DataViewSchema inputSchema);
                var predictionEngine = context.Model.CreatePredictionEngine<TaxiFareInput, TaxiFarePrediction>(model);

                //CMT,1,1,581,5.4,CSH,16.5
                var modelInput = new TaxiFareInput
                {
                    Rate_code = 1,
                    Passenger_count = 1,
                    Trip_time_in_secs = 581,
                    Trip_distance = 5.4F,
                    Payment_type = "CSH",
                    Fare_amount = 0
                };

                var modelOutput = predictionEngine.Predict(modelInput);

                Console.WriteLine("Predicted Fare : " + modelOutput.Score.ToString() + ", " +
                    "Actual Fare : 16.5");
            }
            catch (Exception exceptionObject)
            {
                Console.WriteLine("Error Occurred, Details : " + exceptionObject.Message);
            }

            Console.WriteLine("End of App!");
            Console.ReadLine();
        }
    }
}
