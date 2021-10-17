using ComputerShareCTCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerShareCTCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ImportFile(ResultsViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public IActionResult ImportFile(HomeViewModel model)
        {
            if (model.MyFile == null)
            {
                TempData["Error"] = "No file selected for upload, please select a file.";
                return RedirectToAction("Index", "Home");
            }
            var results = new ResultsViewModel();

            try
            {
                var file = model.MyFile;
                var reader = new StreamReader(file.OpenReadStream());
                var fileText = reader.ReadLine();
                string[] valueArray = fileText.Split(',');
                float[] floatArray = Array.ConvertAll(valueArray, s => float.Parse(s));

                Dictionary<int, float> keyValuePairs = new Dictionary<int, float>();

                for (var i = 0; i < floatArray.Length; i++)
                {
                    keyValuePairs.Add(i, floatArray[i]);
                }

                var maxDifference = 0.00;

                for (var i = 0; i < keyValuePairs.Count(); i++)
                {
                    var staticValue = keyValuePairs[i];

                    for (var j = i + 1; j < keyValuePairs.Count(); j++)
                    {
                        var compareValue = keyValuePairs[j];

                        if (staticValue < compareValue)
                        {
                            var differential = compareValue - staticValue;
                            if (differential > maxDifference)
                            {
                                maxDifference = differential;

                                results.MaxDate = j + 1;
                                results.MaxPrice = compareValue;
                                results.MinDate = i + 1;
                                results.MinPrice = staticValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return View(results);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
