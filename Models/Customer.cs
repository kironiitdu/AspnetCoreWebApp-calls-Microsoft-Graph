using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2_1_Call_MSGraph.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Address CustomerAddress { get; set; }

    }
}
