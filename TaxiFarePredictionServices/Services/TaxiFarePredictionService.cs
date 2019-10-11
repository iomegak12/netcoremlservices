using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxiFarePredictionAppML.Model;

namespace TaxiFarePredictionServices.Services
{
    public class TaxiFarePredictionService : ITaxiFarePredictionService
    {
        private static MLContext context = default(MLContext);
        private static DataViewSchema outputSchema = default(DataViewSchema);
        private static ITransformer pipeline = default(ITransformer);
        private static PredictionEngine<TaxiFareInput, TaxiFarePrediction> predictionEngine = default(PredictionEngine<TaxiFareInput, TaxiFarePrediction>);

        static TaxiFarePredictionService()
        {
            var modelFile = AppDomain.CurrentDomain.BaseDirectory + "MLModel.zip";

            context = new MLContext(seed: default(int));
            pipeline = context.Model.Load(filePath: modelFile, out outputSchema);
            predictionEngine = context.Model.CreatePredictionEngine<TaxiFareInput, TaxiFarePrediction>(pipeline);
        }

        public TaxiFarePrediction GetTaxiFare(TaxiFareInput taxiFareInput)
        {
            var validation = taxiFareInput != default(TaxiFareInput) &&
                taxiFareInput.Passenger_count >= 1 &&
                taxiFareInput.Trip_distance >= 1 &&
                taxiFareInput.Trip_time_in_secs >= 1;

            if (!validation)
                throw new ArgumentException("Invalid Taxi Fare Input Specified!");

            var prediction = predictionEngine.Predict(taxiFareInput);

            return prediction;
        }
    }
}
