using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ApiFileResponse Get()
        {
            string folderName = "/serverdata";

            System.IO.Directory.CreateDirectory(folderName);

            string fileName = "foo.txt";

            var pathString = System.IO.Path.Combine(folderName, fileName);

            Console.WriteLine("Path to my file: {0}\n", pathString);

            // Generate random txt file
            var buffer = new byte[1000];
            new Random().NextBytes(buffer);
            var text = Encoding.Default.GetString(buffer);
            System.IO.File.WriteAllText(pathString, text);

            var bytes = System.IO.File.ReadAllBytes(pathString);
            var file = Convert.ToBase64String(bytes);

            return new ApiFileResponse
            {
                FileAsBase64 = file,
                CheckSum = GetCheckSum(pathString)
            };
        }

        private string GetCheckSum(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
