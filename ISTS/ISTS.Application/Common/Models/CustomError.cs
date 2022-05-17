using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Models
{
    public class CustomError
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public int ErrorCode { get; set; }
    }
}
