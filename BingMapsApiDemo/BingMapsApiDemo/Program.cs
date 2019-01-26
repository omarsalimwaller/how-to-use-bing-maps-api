using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BingMapsRESTToolkit;

namespace BingMapsApiDemo
{
    /// <summary>
    /// How to use bing maps API
    /// 1/26/19
    /// Omar Waller
    /// 
    /// Description: This .NET Framework console application uses the Bing Maps API to locate a registered address given
    /// the users address as input. 
    /// Credit to William Leaneah at Marquette University for some guidance.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Escape to exit, enter to continue");

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Console.Clear();
                // INFO: Get user address
                Console.WriteLine("What is your address?");
                String address = Console.ReadLine();

                // INFO: Create service proxy client and service request
                var request = ServiceManager.GetResponseAsync(

                    new GeocodeRequest()
                    {
                        BingMapsKey = System.Configuration.ConfigurationManager.AppSettings.Get("BingMapsKey"),
                        Query = address
                    }).GetAwaiter().GetResult();

                //INFO: Store bounded location strings, used for each location.
                List<string> corners = new List<string> { "South Latitude", "West Longitude", "North Latitude", "East Longitude" };

                //INFO: GetAwaiter() function allows you to wait for a single asynchronous task to complete
                //INFO: GetResult() function ends the wait for the completion of the asynchronous task
                for (int item = 0; item < request.ResourceSets[0].Resources.Length; item++)
                {
                    //INFO: Cast location result as Location type so we can print.
                    Location location = request.ResourceSets[0].Resources[item] as Location;

                    //INFO: Print to user
                    Console.WriteLine("Address " + (item + 1));
                    Console.WriteLine("Formatted Address: " + location.Address.FormattedAddress.ToString());
                    Console.WriteLine("Latitude: " + location.Point.Coordinates[0].ToString());
                    Console.WriteLine("Longitude: " + location.Point.Coordinates[1].ToString());
                    Console.WriteLine("Confidence this is your location: " + location.Confidence.ToString());
                    Console.WriteLine("Bounded Area of Location");

                    //INFO: Loop through bounded box coordinates and print.
                    for (int box_corner = 0; box_corner < location.BoundingBox.Length; box_corner++)
                    {
                        Console.WriteLine("        " + corners[box_corner] + " Corner: " + location.BoundingBox[box_corner].ToString());
                    };

                    Console.WriteLine();

                }



            }
        }
    }
}
