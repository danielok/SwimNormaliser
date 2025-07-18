using System.Runtime.Serialization;
using Math.Gps;
using Math.Tools.TrackReaders;
using SwimNormaliser.Services;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Swim File Normaliser");


var sampleFilesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample Files");
if (!Directory.Exists(sampleFilesDirectory))
{
    Console.WriteLine($"Sample Files directory does not exist: {sampleFilesDirectory}");
    return;
}
else
{
    
    var firstActivityIds = TcxFileReader.GetFirstActivityIds(sampleFilesDirectory);
    foreach (var kvp in firstActivityIds)
    {
        Console.WriteLine($"File: {kvp.Key}, First Activity ID: {kvp.Value}");
    }
}

Console.WriteLine("Press any key to exit...");
Console.ReadLine();
Console.WriteLine("Exiting...");