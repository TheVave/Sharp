using SharpPhysics;

_2dBaseObjects.LoadSquare();
MainRenderer.StartRenderer(false);
bool startSimMessage = false;
for (int i = 0; i < 17000; i++)
{
	_2dSimulatedObject obj = _2dBaseObjects.Square;
	new Thread(() =>
	{
		while (true)
		{
			Task.Delay(1000).Wait();
			if (startSimMessage)
			{
				obj.StartPhysicsSimulation();
				break;
			}
		}
	}).Start();
	Console.WriteLine(i);
}
Console.WriteLine("creation complete.");
Console.ReadKey();
Console.WriteLine("Starting Physics sim.");
startSimMessage = true;
Console.ReadLine();
GlobalDeclerations.IsSolvingPhysics = false;
Console.ReadLine();