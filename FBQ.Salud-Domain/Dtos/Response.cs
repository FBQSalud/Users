using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Domain.Dtos
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Object Result { get; set; }
    }
}
