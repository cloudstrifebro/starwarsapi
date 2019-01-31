using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PlattSampleApp.Interfaces;
using PlattSampleApp.Models.ViewModels;

namespace PlattSampleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStarWarsService _starWarsService;
        private readonly IMapper _mapper;

        public HomeController(IStarWarsService starWarsService, IMapper mapper)
        {
            _starWarsService = starWarsService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetAllPlanets(CancellationToken cancellationToken)
        {
            // TODO: Implement this controller action
            AllPlanetsViewModel vm = null;
            try
            {
                vm = await _starWarsService.GetAllPlanetsAsync(cancellationToken);
            }
            catch (NullReferenceException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                //potentially log error here and return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                //potentially log error and rethrow error, or, in this case, just return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return View(vm);
        }

        public async Task<ActionResult> GetPlanetTwentyTwo(CancellationToken cancellationToken, int planetid)
        {
            // TODO: Implement this controller action
            SinglePlanetViewModel vm = null;

            try
            {
                vm = await _starWarsService.GetPlanetTwentyTwoAsync(planetid, cancellationToken);
            }
            catch (NullReferenceException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                //potentially log error here and return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                //potentially log error and rethrow error, or, in this case, just return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return View(vm);
        }

        public async Task<ActionResult> GetResidentsOfPlanetNaboo(CancellationToken cancellationToken, string planetName)
        {
            // TODO: Implement this controller action            
            PlanetResidentsViewModel vm = null;

            try
            {
                vm = await _starWarsService.GetResidentsOfPlanetNabooAsync(planetName, cancellationToken);
            }
            catch (NullReferenceException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                //potentially log error here and return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                //potentially log error and rethrow error, or, in this case, just return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return View(vm);
        }

        public async Task<ActionResult> VehicleSummary(CancellationToken cancellationToken)
        {
            // TODO: Implement this controller action            
            VehicleSummaryViewModel vm = null;

            try
            {
                vm = await _starWarsService.VehicleSummaryAsync(cancellationToken);
            }
            catch (NullReferenceException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                //potentially log error here and return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                //potentially log error and rethrow error, or, in this case, just return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return View(vm);
        }

        public async Task<ActionResult> GetHanSolo(CancellationToken cancellationToken)
        {
            ResidentSummary vm = null;

            try
            {
                vm = await _starWarsService.GetHanSoloAsync(cancellationToken);
            }
            catch (NullReferenceException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                //potentially log error here and return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                //potentially log error and rethrow error, or, in this case, just return 500
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            return View(vm);
        }
    }
}