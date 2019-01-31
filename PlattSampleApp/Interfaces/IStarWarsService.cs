using PlattSampleApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PlattSampleApp.Interfaces
{
    public interface IStarWarsService
    {
        Task<AllPlanetsViewModel> GetAllPlanetsAsync(CancellationToken cancellationToken);
        Task<SinglePlanetViewModel> GetPlanetTwentyTwoAsync(int planetId, CancellationToken cancellationToken);
        Task<PlanetResidentsViewModel> GetResidentsOfPlanetNabooAsync(string planetName, CancellationToken cancellationToken);
        Task<VehicleSummaryViewModel> VehicleSummaryAsync(CancellationToken cancellationToken);
        Task<ResidentSummary> GetHanSoloAsync(CancellationToken cancellationToken);
    }
}