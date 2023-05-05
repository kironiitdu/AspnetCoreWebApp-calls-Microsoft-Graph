using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2_1_Call_MSGraph.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string HouseName { get; set; }
        public Address Location { get; set; }
    }
}
