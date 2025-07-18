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

        double oldTotalMeters = 0;
        double newTotalMeters = 0;

        if (average.HasValue)
        {
            Console.WriteLine(
                                    "{0,12}  \t{1,12}  \t{2,12}  ",
                                    "Time", "Distance", "New Distance"
                                );
            TcxFileReader.GetLapDistancesAndTimes(file)
                .ForEach(lap =>
                {
                    // Calculate lap speed
                    double lapSpeed = (lap.TotalTimeSeconds > 0) ? lap.DistanceMeters / lap.TotalTimeSeconds : 0;
                    double newDistance = lap.DistanceMeters;

                    // Only recalculate if lap speed is more than 20% faster or slower than average
                    if (average.HasValue && lap.TotalTimeSeconds > 0 && lap.DistanceMeters > 0)
                    {
                        if (lapSpeed > average.Value * 1.3 || lapSpeed < average.Value * 0.7)
                        {
                            newDistance = Math.Floor((average.Value * lap.TotalTimeSeconds) / 50) * 50;
                        }
                    }

                    oldTotalMeters += lap.DistanceMeters;
                    newTotalMeters += newDistance;
                    // Use fixed-width formatting for tab stops
                    Console.WriteLine(
                        "{0,12} s\t{1,12} m\t{2,12} m",
                        lap.TotalTimeSeconds, lap.DistanceMeters, newDistance
                    );
                });

            Console.WriteLine($"Old Total Meters: {oldTotalMeters} m");
            Console.WriteLine($"New Total Meters: {newTotalMeters} m");

            Console.WriteLine("\n--------------------------------------------------\n");

        }
        else
        {
            Console.WriteLine("No average speed found for this file.");
        }
    }
}

//Console.WriteLine("Press any key to exit...");
//Console.ReadLine();
Console.WriteLine("Exiting...");