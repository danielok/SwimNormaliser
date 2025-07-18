using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SwimNormaliser.Services
{

    public class TcxFileReader
    {
        public TcxFileReader(string filename)
        {

        }

        /// <summary>
        /// Opens each TCX file from the Sample Files directory and returns the value of the ID element
        /// from the first Activity element in the Activities element.
        /// </summary>
        /// <param name="sampleFilesDirectory">The path to the Sample Files directory.</param>
        /// <returns>A dictionary mapping file names to the first Activity ID value.</returns>
        public static Dictionary<string, string?> GetFirstActivityIds(string sampleFilesDirectory)
        {
            var result = new Dictionary<string, string?>();
            var tcxFiles = Directory.GetFiles(sampleFilesDirectory, "*.tcx");

            XNamespace ns = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2";

            foreach (var file in tcxFiles)
            {
                try
                {
                    var doc = XDocument.Load(file);
                    var activities = doc.Root?.Element(ns + "Activities");
                    var firstActivity = activities?.Element(ns + "Activity");
                    var idElement = firstActivity?.Element(ns + "Id");
                    result[Path.GetFileName(file)] = idElement?.Value;
                }
                catch (Exception ex)
                {
                    result[Path.GetFileName(file)] = $"Error: {ex.Message}";
                }
            }

            return result;
        }

        /// <summary>
        /// Opens a TCX file and returns a list of (DistanceMeters, TotalTimeSeconds) for each Lap.
        /// </summary>
        /// <param name="tcxFilePath">The path to the TCX file.</param>
        /// <returns>List of tuples (DistanceMeters, TotalTimeSeconds) for each Lap.</returns>
        public static List<(double DistanceMeters, double TotalTimeSeconds)> GetLapDistancesAndTimes(string tcxFilePath)
        {
            var result = new List<(double DistanceMeters, double TotalTimeSeconds)>();
            XNamespace ns = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2";

            var doc = XDocument.Load(tcxFilePath);
            var laps = doc.Descendants(ns + "Lap");

            foreach (var lap in laps)
            {
                var distanceElem = lap.Element(ns + "DistanceMeters");
                var timeElem = lap.Element(ns + "TotalTimeSeconds");

                if (distanceElem != null && timeElem != null &&
                    double.TryParse(distanceElem.Value, out double distance) &&
                    double.TryParse(timeElem.Value, out double time))
                {
                    result.Add((distance, time));
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the average speed (meters per second) for a TCX file by summing the DistanceMeters of all laps where DistanceMeters > 0
        /// and dividing by the sum of TotalTimeSeconds for those laps.
        /// </summary>
        /// <param name="tcxFilePath">The path to the TCX file.</param>
        /// <returns>The average speed in meters per second, or null if no valid laps are found.</returns>
        public static double? GetAverageLapSpeed(string tcxFilePath)
        {
            XNamespace ns = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2";
            var doc = XDocument.Load(tcxFilePath);
            var laps = doc.Descendants(ns + "Lap");

            double totalDistance = 0;
            double totalTime = 0;

            foreach (var lap in laps)
            {
                var distanceElem = lap.Element(ns + "DistanceMeters");
                var timeElem = lap.Element(ns + "TotalTimeSeconds");

                if (distanceElem != null && timeElem != null &&
                    double.TryParse(distanceElem.Value, out double distance) &&
                    double.TryParse(timeElem.Value, out double time) &&
                    distance > 0)
                {
                    totalDistance += distance;
                    totalTime += time;
                }
            }

            if (totalTime > 0)
                return totalDistance / totalTime;
            else
                return null;
        }
    }
}