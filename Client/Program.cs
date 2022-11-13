using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;

namespace Client
{
    class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var filePath = "/clientdata/foo.txt";
            var endpoint = string.IsNullOrEmpty(args[0]) || args[0] == "arg0" ? "host.docker.internal:3500" : args[0];
            using (HttpClient client = new HttpClient())
            {
                var result = client.GetAsync($"http://{endpoint}/File");

                result.Wait();

                var resultString = result.Result.Content.ReadAsStringAsync();

                resultString.Wait();

                var apiResult = JsonConvert.DeserializeObject<ApiResult>(resultString.Result);

                System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(apiResult.FileAsBase64));

                try
                {
                    var readBuffer = System.IO.File.ReadAllText(filePath);
                    Console.WriteLine(readBuffer);

                    if(apiResult.CheckSum == GetCheckSum(filePath))
                    {
                        Console.WriteLine("Checksums match, transfer complete");
                    }

                    else
                    {
                        Console.WriteLine("ERROR: Checksums don't match");
                    }
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.CancelKeyPress += (sender, eArgs) => {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };

            _quitEvent.WaitOne();
        }

        private static string GetCheckSum(string filename)
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

        public class ApiResult
        {
            public string FileAsBase64 { get; set; }
            public string CheckSum { get; set; }
        }
    }
}
