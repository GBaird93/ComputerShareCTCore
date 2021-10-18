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

        // run Index on startup
        public IActionResult Index()
        {
            return View();
        }

        // get information from ResultsViewModel
        public IActionResult ImportFile(ResultsViewModel model)
        {
            return View(model);
        }

        // Takes in file
        // processes file contents through algorithm
        // returns value to ResultsViewModel
        [HttpPost]
        public IActionResult ImportFile(HomeViewModel model)
        {
            // check MyFile property in home view model to ensure user has selected a file
            if (model.MyFile == null)
            {
                //if no file is found, return error message to user and redirect to Index view
                TempData["Error"] = "No file selected for upload, please select a file.";
                return RedirectToAction("Index", "Home");
            }

            // instantiate new viewmodel
            var results = new ResultsViewModel();


            try
            {
                // set new variable to MyFile contents in model
                var file = model.MyFile;
                // set new variable to open .txt file
                var reader = new StreamReader(file.OpenReadStream());
                // set new variable to read each line of text file
                var fileText = reader.ReadLine();
                // convert contents of text file into a string array
                // & use a comma to split each value
                string[] valueArray = fileText.Split(',');
                // convert contents of string array into floats
                // & store them in a float array
                float[] floatArray = Array.ConvertAll(valueArray, s => float.Parse(s));

                // Instantiate a new dictionary of key value pairs
                Dictionary<int, float> keyValuePairs = new Dictionary<int, float>();

                // for each item in the float array...
                for (var i = 0; i < floatArray.Length; i++)
                {
                    // add each item to keyValuePairs dictionary
                    keyValuePairs.Add(i, floatArray[i]);
                }

                // set new float variable called maxDifference
                var maxDifference = 0.00;

                // for every item in dictionary....
                for (var i = 0; i < keyValuePairs.Count(); i++)
                {
                    // create new static variable to stay on first value of dictionary
                    // until full cycle has been completed
                    var staticValue = keyValuePairs[i];

                    // nested for loop for each item in dictionary....
                    for (var j = i + 1; j < keyValuePairs.Count(); j++)
                    {
                        // create new variable to allow comparison of multiple values in dictionary
                        var compareValue = keyValuePairs[j];

                        // if first value in dictionary is less than the next value....
                        if (staticValue < compareValue)
                        {
                            // create new variable and set its value to 
                            // the highest compared value - lowest compared value
                            var differential = compareValue - staticValue;
                            // if the differential is greater than the
                            // max difference....
                            if (differential > maxDifference)
                            {
                                // set the max difference to value of current differential
                                maxDifference = differential;

                                // set MaxDate key to j+1 (to account for key value pairs starting at 0)
                                results.MaxDate = j + 1;
                                // set MaxPrice value to compareValue
                                results.MaxPrice = compareValue;
                                // set MinDate key to i+1 (to account for key value pairs starting at 0)
                                results.MinDate = i + 1;
                                // set MinPrice value to staticValue
                                results.MinPrice = staticValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            // return results to view
            return View(results);

        }
    }
}
