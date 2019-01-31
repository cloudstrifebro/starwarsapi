using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PlattSampleApp.Models.DataTransferObjects
{
    [Serializable, DataContract]
    public class PeopleDto
    {        
        [DataMember(Name ="name")]
        public string Name { get; set; }
        [DataMember(Name = "height")]
        public string Height { get; set; }
        [DataMember(Name = "mass")]
        public string Mass { get; set; }
        [DataMember(Name = "hair_color")]
        public string HairColor { get; set; }
        [DataMember(Name = "skin_color")]
        public string SkinColor { get; set; }
        [DataMember(Name = "eye_color")]
        public string EyeColor { get; set; }
        [DataMember(Name = "birth_year")]
        public string BirthYear { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "homeworld")]
        public string Homeworld { get; set; }
        [DataMember(Name = "films")]
        public List<string> Films { get; set; }
        [DataMember(Name = "species")]
        public List<string> Species { get; set; }
        [DataMember(Name = "vehicles")]
        public List<string> Vehicles { get; set; }
        [DataMember(Name = "starships")]
        public List<string> Starships { get; set; }
        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
        [DataMember(Name = "edited")]
        public DateTime Edited { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}