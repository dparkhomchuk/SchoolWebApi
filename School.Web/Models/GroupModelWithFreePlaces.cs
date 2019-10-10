using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School.Models;

namespace School.Web.Models
{
    public class GroupModelWithFreePlaces:GroupModel
    {
        public int FreePlaces { get; set; }
    }
}
