using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PlattSampleApp.Models.DataTransferObjects
{
    [Serializable, DataContract]
    public class StarWarsRequestDto
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "next")]
        public string Next { get; set; }
        [DataMember(Name = "previous")]
        public object Previous { get; set; }
        [DataMember(Name = "results")]
        public IEnumerable<object> Results { get; set; }
    }
}