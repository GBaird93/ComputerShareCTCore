using Microsoft.AspNetCore.Http;
using System;

namespace ComputerShareCTCore.Models
{
    public class ResultsViewModel
    {
        public float MaxPrice { get; set; }
        public int MaxDate { get; set; }
        
        public float MinPrice { get; set; }
        public int MinDate { get; set; }

    }
}
