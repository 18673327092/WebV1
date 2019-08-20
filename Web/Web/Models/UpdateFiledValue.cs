using Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class UpdateFiledValue
    {
        public int id { get; set; }
        public string value { get; set; }
        public string field { get; set; }
    }
}