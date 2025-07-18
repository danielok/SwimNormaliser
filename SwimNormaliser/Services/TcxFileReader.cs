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
    }
}