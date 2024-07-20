
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var service = new SwimNormaliser.Services.TcxFileReader("example.tcx");

Console.WriteLine(service.GetType().Name);

Console.ReadLine();

return;