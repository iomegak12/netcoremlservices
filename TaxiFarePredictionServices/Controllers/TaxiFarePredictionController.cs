using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxiFarePredictionAppML.Model;
using TaxiFarePredictionServices.Services;

namespace TaxiFarePredictionServices.Controllers
{
    [Route("api/taxi-fare")]
    [ApiController]
    public class TaxiFarePredictionController : ControllerBase
    {
        private ITaxiFarePredictionService taxiFarePredictionService = default(ITaxiFarePredictionService);

        private const int MIN_PASSENGERS = 1;
        private const int MIN_TRIP_DISTANCE = 1;
        private const int MIN_TRIP_IN_SECS = 60;

        public TaxiFarePredictionController(ITaxiFarePredictionService taxiFarePredictionService)
        {
            if (taxiFarePredictionService == default(ITaxiFarePredictionService))
                throw new ArgumentNullException(nameof(taxiFarePredictionService));

            this.taxiFarePredictionService = taxiFarePredictionService;
        }

        [HttpPost]
        [Route(template: "predict")]
        public IActionResult CalculateTaxiFare([FromBody] TaxiFareInput taxiFareInput)
        {
            var validation = this.taxiFarePredictionService != default(ITaxiFarePredictionService) &&
                taxiFareInput != default(TaxiFareInput) &&
                taxiFareInput.Passenger_count >= MIN_PASSENGERS &&
                taxiFareInput.Trip_distance >= MIN_TRIP_DISTANCE &&
                taxiFareInput.Trip_time_in_secs >= MIN_TRIP_IN_SECS;

            if (!validation)
                return new BadRequestResult();

            try
            {
                var taxiFarePrediction = this.taxiFarePredictionService.GetTaxiFare(taxiFareInput);

                return Ok(taxiFarePrediction);
            }
            catch (Exception exceptionObject)
            {
                return new BadRequestObjectResult(exceptionObject.Message);
            }
        }
    }
}