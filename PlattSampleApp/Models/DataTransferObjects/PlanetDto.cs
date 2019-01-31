using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PlattSampleApp.Models.DataTransferObjects
{
    [Serializable, DataContract]
    public class PlanetDto
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "rotation_period")]
        public string RotationPeriod { get; set; }
        [DataMember(Name = "orbital_period")]
        public string OrbitalPeriod { get; set; }
        [DataMember(Name = "diameter")]
        public string Diameter { get; set; }
        [DataMember(Name = "climate")]
        public string Climate { get; set; }
        [DataMember(Name = "gravity")]
        public string Gravity { get; set; }
        [DataMember(Name = "terrain")]
        public string Terrain { get; set; }
        [DataMember(Name = "surface_water")]
        public string SurfaceWater { get; set; }
        [DataMember(Name = "population")]
        public string Population { get; set; }
        [DataMember(Name = "residents")]
        public List<string> Residents { get; set; }
        [DataMember(Name = "films")]
        public List<string> Films { get; set; }
        [DataMember(Name = "created")]
        public DateTime created { get; set; }
        [DataMember(Name = "edited")]
        public DateTime Edited{ get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}