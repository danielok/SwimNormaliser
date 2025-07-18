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

    var tcxFiles = Directory.GetFiles(sampleFilesDirectory, "*.tcx");


    foreach (var file in tcxFiles)
    {
        Console.WriteLine($"Processing file: {Path.GetFileName(file)}");


        var average = TcxFileReader.GetAverageLapSpeed(file);
            Console.WriteLine("Average Lap Speed: " + average + " m/s");

        TcxFileReader.GetLapDistancesAndTimes(file)
            .ForEach(lap => Console.WriteLine($"Distance: {lap.DistanceMeters} m \t\t Time: {lap.TotalTimeSeconds} s"));

        Console.WriteLine("\n--------------------------------------------------\n");

    }
}

//Console.WriteLine("Press any key to exit...");
//Console.ReadLine();
Console.WriteLine("Exiting...");