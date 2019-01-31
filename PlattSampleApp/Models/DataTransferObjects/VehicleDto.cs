using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PlattSampleApp.Models.DataTransferObjects
{
    [Serializable, DataContract]
    public class VehicleDto
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "model")]
        public string Model { get; set; }
        [DataMember(Name = "manufacturer")]
        public string Manufacturer { get; set; }
        [DataMember(Name = "cost_in_credits")]
        public string CostInCredits { get; set; }
        [DataMember(Name = "length")]
        public string Length { get; set; }
        [DataMember(Name = "max_atmosphering_speed ")]
        public string MaxAtmospheringSpeed { get; set; }
        [DataMember(Name = "crew")]
        public string Crew { get; set; }
        [DataMember(Name = "passengers")]
        public string Passengers { get; set; }
        [DataMember(Name = "cargo_capacity")]
        public string CargoCapacity { get; set; }
        [DataMember(Name = "consumables")]
        public string Consumables { get; set; }
        [DataMember(Name = "vehicle_class")]
        public string VehicleClass { get; set; }
        [DataMember(Name = "pilots")]
        public List<object> Pilots { get; set; }
        [DataMember(Name = "films")]
        public List<string> Films { get; set; }
        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
        [DataMember(Name = "edited")]
        public DateTime Edited { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}