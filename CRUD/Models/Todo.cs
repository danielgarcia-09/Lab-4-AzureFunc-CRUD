using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Models
{
    public class Todo
    {
        public int Id { get; set; }

        public string Task { get; set; }

        public bool Done { get; set; }

        public string CreatedAt { get; set; }
    }
}
