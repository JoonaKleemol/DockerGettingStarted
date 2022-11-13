using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ApiFileResponse
    {
        public string FileAsBase64 { get; set; }
        public string CheckSum { get; set; }
    }
}
