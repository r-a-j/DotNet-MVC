using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUD_Practice.Models
{
    public class FishViewModel
    {
        public int FishId { get; set; }
        public string FishName { get; set; }
        public string FishType { get; set; }
        public int FishRate { get; set; }
        public int FishQuantity { get; set; }
    }
}