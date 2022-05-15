// See https://aka.ms/new-console-template for more information
using Gort.Data;
using Gort.Data.DataModel;

var ctx = new GortContext();
//Gort.Data.Load.InstanceLoader.LoadStatics(ctx);
Gort.Data.Load.InstanceLoader.LoadWorkSpace(ctx);
Console.WriteLine("Goodbye, World!");