using AutoMapper;
using MoreLinq;
using Newtonsoft.Json;
using PlattSampleApp.Interfaces;
using PlattSampleApp.Models.DataTransferObjects;
using PlattSampleApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PlattSampleApp.Services
{
    /*
     * Note to reviewers: 
     * I'm using morelinq to enhance the default .NET LINQ library.  
     * By default, you can't do DistinctBy without implementing an equality comparer.
     * I'm lazy, so I'd rather use something someone smarter than me wrote.  
     * 
     * "Work smarter, not harder." -Scrooge McDuck
     * */

    public class StarWarsService : IStarWarsService
    {
        const string API_ROOT = "https://swapi.co/api";
        static string PeopleBaseUri = $"{API_ROOT}/people";
        static string VehiclesBaseUri = $"{API_ROOT}/vehicles";
        static string PlanetsBaseUri = $"{API_ROOT}/planets";
        private readonly IMapper _mapper;

        public StarWarsService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<AllPlanetsViewModel> GetAllPlanetsAsync(CancellationToken cancellationToken)
        {
            var results = new List<PlanetDto>();
            var data = await GetRequestAsync(PlanetsBaseUri, cancellationToken);
            var requestDto = JsonConvert.DeserializeObject<StarWarsRequestDto>(data);

            if (requestDto == null)
            {
                throw new NullReferenceException("No planets found.");
            }

            await PopulateRequestResults(requestDto, results, cancellationToken);

            var planetDetails = new List<PlanetDetailsViewModel>();

            var orderedResults = results.OrderByDescending(p => p.Diameter).ToList();

            foreach (var detail in orderedResults)
            {
                try
                {
                    planetDetails.Add(new PlanetDetailsViewModel
                    {
                        Diameter = detail.Diameter == "unknown" ? 0 : int.Parse(detail.Diameter),
                        LengthOfYear = detail.OrbitalPeriod,
                        Name = detail.Name,
                        Population = detail.Population,
                        Terrain = detail.Terrain
                    });
                }
                catch (Exception ex)
                {
                    //log the error if necessary
                    Debug.WriteLine(ex);
                    //rethrow the exception so we can see what happened
                    throw;
                }
            }

            //order planets from largest to smallest
            return new AllPlanetsViewModel
            {
                AverageDiameter = orderedResults.Where(p => p.Diameter != "unknown").Average(p => double.Parse(p.Diameter)),
                Planets = planetDetails.OrderByDescending(p => p.Diameter).ToList()
            };
        }

        public async Task<ResidentSummary> GetHanSoloAsync(CancellationToken cancellationToken)
        {
            var people = new List<PeopleDto>();

            var peopleData = await GetRequestAsync($"{PeopleBaseUri}/14", cancellationToken);

            //should never happen, but if URI is unavailable, e.g., server is down, shouldn't work
            if (string.IsNullOrWhiteSpace(peopleData))
            {
                throw new NullReferenceException("I couldn't find Han. Sorry - he died in the Disney release.  Thanks Disney for killing the coolest character. >:(");
            }

            var peopleRequestDto = JsonConvert.DeserializeObject<PeopleDto>(peopleData);

            return new ResidentSummary
            {
                Name = peopleRequestDto.Name,
                EyeColor = peopleRequestDto.EyeColor,
                Gender = peopleRequestDto.Gender,
                HairColor = peopleRequestDto.HairColor,
                Height = peopleRequestDto.Height,
                SkinColor = peopleRequestDto.SkinColor,
                Weight = peopleRequestDto.Mass
            };
        }

        public async Task<SinglePlanetViewModel> GetPlanetTwentyTwoAsync(int planetId, CancellationToken cancellationToken)
        {
            var results = new List<PlanetDto>();
            var data = await GetRequestAsync($"{PlanetsBaseUri}/{planetId}", cancellationToken);

            if (string.IsNullOrWhiteSpace(data))
            {
                throw new NullReferenceException($"{planetId} not found.");
            }

            var requestDto = JsonConvert.DeserializeObject<PlanetDto>(data);

            return new SinglePlanetViewModel
            {
                Climate = requestDto.Climate,
                Diameter = requestDto.Diameter,
                Gravity = requestDto.Gravity,
                LengthOfDay = requestDto.RotationPeriod,
                LengthOfYear = requestDto.OrbitalPeriod,
                Name = requestDto.Name,
                Population = requestDto.Population,
                SurfaceWaterPercentage = requestDto.SurfaceWater
            };
        }

        public async Task<PlanetResidentsViewModel> GetResidentsOfPlanetNabooAsync(string planetName, CancellationToken cancellationToken)
        {
            var people = new List<PeopleDto>();
            var planets = new List<PlanetDto>();

            var peopleData = await GetRequestAsync($"{PeopleBaseUri}", cancellationToken);
            var planetData = await GetRequestAsync($"{PlanetsBaseUri}", cancellationToken);

            //should never happen, but if URI is unavailable, e.g., server is down, shouldn't work
            if (string.IsNullOrWhiteSpace(peopleData))
            {
                throw new NullReferenceException("No people found.");
            }

            if (string.IsNullOrWhiteSpace(planetData))
            {
                throw new NullReferenceException("No planets found.");
            }

            var peopleRequestDto = JsonConvert.DeserializeObject<StarWarsRequestDto>(peopleData);
            var planetRequestDto = JsonConvert.DeserializeObject<StarWarsRequestDto>(planetData);

            //try to populate the result list with the respective data.
            //This could be rewritten to ONLY query for the selected planet
            //as opposed to populating an entire list.  Done here for lack of time.
            //General pseudocode for the interested:
            /*
             * 
                Search for planet by name
                If planet name is found while querying, stop request (maybe with a cancellation token)
                return found planet
                Else - if planet name not found, quit and throw an error or something
            */
            await PopulateRequestResults(peopleRequestDto, people, cancellationToken);
            await PopulateRequestResults(planetRequestDto, planets, cancellationToken);

            //get selected planet, e.g., "Naboo"
            var selectedPlanet = planets.FirstOrDefault(p => p.Name == planetName);

            //planet wasn't found; throw an error
            if (selectedPlanet == null)
            {
                throw new NullReferenceException($"{planetName} was not found.");
            }

            var selectedPeople = people.Where(p => p.Homeworld == selectedPlanet.Url).ToList();

            var residents = new List<ResidentSummary>();

            foreach (var resident in selectedPeople)
            {
                residents.Add(new ResidentSummary
                {
                    EyeColor = resident.EyeColor,
                    Gender = resident.Gender,
                    HairColor = resident.HairColor,
                    Height = resident.Height,
                    Name = resident.Name,
                    SkinColor = resident.SkinColor,
                    Weight = resident.Mass,
                });
            }

            return new PlanetResidentsViewModel
            {
                Residents = residents.OrderBy(r => r.Name).ToList(),
            };
        }

        public async Task<VehicleSummaryViewModel> VehicleSummaryAsync(CancellationToken cancellationToken)
        {
            var vehicles = new List<VehicleDto>();

            var data = await GetRequestAsync($"{VehiclesBaseUri}", cancellationToken);
            var requestDto = JsonConvert.DeserializeObject<StarWarsRequestDto>(data);

            await PopulateRequestResults(requestDto, vehicles, cancellationToken);            

            //get distinct manufacturers
            var distinctManufacturerVehicles = vehicles
                .Where(v => !string.IsNullOrWhiteSpace(v.CostInCredits) && v.CostInCredits != "unknown")
                .DistinctBy(v => v.Manufacturer).ToList();

            //get all vehicles that are known
            var vehiclesWithoutUnknown = vehicles
                .Where(v => !string.IsNullOrWhiteSpace(v.CostInCredits) && v.CostInCredits != "unknown")
                .ToList();

            IDictionary<string, int> vehicleCountDict = vehiclesWithoutUnknown.CountBy(vNoUnk => vNoUnk.Manufacturer).ToDictionary();

            //Project data into new collection; calculate the average cost by manufacturer; 
            //find the count that was returned from the dictionary
            var stats = distinctManufacturerVehicles.Select(v => new VehicleStatsViewModel {
                AverageCost = vehiclesWithoutUnknown
                    .Where(vNoUnk => vNoUnk.Manufacturer == v.Manufacturer)
                    .Average(vNoUnk => double.Parse(vNoUnk.CostInCredits)),
                ManufacturerName = v.Manufacturer,
                VehicleCount = vehicleCountDict[v.Manufacturer]                
            }).ToList();

            return new VehicleSummaryViewModel
            {
                Details = stats.OrderByDescending(v => v.VehicleCount)
                                .ThenByDescending(v => v.AverageCost).ToList(),
                ManufacturerCount = distinctManufacturerVehicles.Count(),
                VehicleCount = vehiclesWithoutUnknown.Count(),
            };

        }

        //THE BELOW ARE HELPER METHODS - THESE SHOULD BE IN ANOTHER SERVICE

        /// <summary>
        /// Helper method used to retrieve an HTTP GET request.
        /// 
        /// Ideally, another service.
        /// </summary>
        /// <param name="uri">The HTTP GET URI</param>
        /// <param name="cancellationToken">The cancellation token for handling cancellations</param>
        /// <returns>Returns a string representing the HTTP request body.</returns>
        private async Task<string> GetRequestAsync(string uri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead, cancellationToken);

                if (response == null)
                {
                    //some weird error happened; log it or something
                    return null;
                }

                return await UnwrapResponseBody(response);
            }
        }

        /// <summary>
        /// Helper method used to unwrap an HttpResponseMessage.
        /// 
        /// Ideally, another service.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>Returns a string representing the HTTP request body.</returns>
        private async Task<string> UnwrapResponseBody(HttpResponseMessage response)
        {
            string item = null;
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    {
                        item = await response.Content.ReadAsStringAsync();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return item;
        }

        /// <summary>
        /// Helper method that is used to populate the results.
        /// 
        /// Ideally, this would probably go in another service.
        /// </summary>
        /// <typeparam name="T">Type to map to</typeparam>
        /// <param name="requestDto">The request data transfer object</param>
        /// <param name="results">The results to populate</param>
        /// <param name="cancellationToken">The cancellation token used for handling cancellations</param>
        /// <returns>Returns a task.</returns>
        private async Task PopulateRequestResults<T>(StarWarsRequestDto requestDto, IList<T> results, CancellationToken cancellationToken)
        {
            var isDone = false;

            if (requestDto == null)
            {
                return;
            }

            while (!isDone)
            {
                if (requestDto == null)
                {
                    isDone = true;
                    continue;
                }

                foreach (var item in requestDto.Results)
                {
                    Debug.WriteLine(item);
                    var newItem = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(item));
                    results.Add(newItem);
                }

                if (requestDto == null || requestDto.Results == null || requestDto.Next == null)
                {
                    isDone = true;
                    continue;
                }

                var data = await GetRequestAsync(requestDto.Next, cancellationToken);
                requestDto = JsonConvert.DeserializeObject<StarWarsRequestDto>(data);
            }
        }


    }
}