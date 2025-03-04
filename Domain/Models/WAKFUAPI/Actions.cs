using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.WAKFUAPI
{
    public class Definition
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("effect")]
        public string Effect { get; set; }
    }

    public class Description
    {
        [JsonProperty("fr")]
        public string French { get; set; }

        [JsonProperty("en")]
        public string English { get; set; }

        [JsonProperty("es")]
        public string Spanish { get; set; }

        [JsonProperty("pt")]
        public string Portuguese { get; set; }
    }

    public class Actions
    {
        [JsonProperty("definition")]
        public Definition Definition { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        public override string ToString()
        {
            return $"[{Definition.Id}] " + Description.Spanish;
        }
    }
}
