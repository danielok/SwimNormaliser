using Math.Gps;
using Math.Tools.TrackReaders;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var service = new SwimNormaliser.Services.TcxFileReader("example.tcx");

Console.WriteLine(service.GetType().Name);



Console.ReadLine();

var x = Math.Tools.TrackReaders.Deserializer.File("");


return;