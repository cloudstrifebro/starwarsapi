using AutoMapper;
using PlattSampleApp.Models.DataTransferObjects;
using PlattSampleApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlattSampleApp.Mappings
{
    public class StarWarsMaps : Profile
    {
        public StarWarsMaps()
        {
            //Use this to create maps for your app.
            //CreateMap<PlanetDetailsViewModel, PlanetDto>()
            //    .ForMember(o => o.Name, ex => ex.MapFrom(o => o.Name))
            //    .ForMember(o => o.Population, ex => ex.MapFrom(o => o.Population))
            //    .ForMember(o => o.Terrain, ex => ex.MapFrom(o => o.Terrain))
            //    .ForMember(o => o.OrbitalPeriod, ex => ex.MapFrom(o => o.LengthOfYear))
            //.ReverseMap();
        }
    }
}