using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD_Practice.Models
{
    public class FishSeedViewModel
    {
        public int FishSeedId { get; set; }
        public string FishSeedName { get; set; }
        public string FishSeedType { get; set; }
        public int FishSeedRate { get; set; }
        public int FishSeedQuantity { get; set; }
    }
}