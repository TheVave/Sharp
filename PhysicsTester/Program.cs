using System;
using SharpPhysics;

namespace SharpShysicsTester
{
    class Program
	{
		static void Main(string[] args)
		{
			SimulatedObject toSimulate = BaseObjects.MakeCube();
			int index = 0;
			while (index < 64)
			{
				index++;
				PhysicsSimulator physicsSimulator = toSimulate.StartPhysicsSimulation();

				//Thread thread = new Thread(() =>
				//{
				//	while (true)
				//	{
				//		Console.WriteLine(physicsSimulator.ObjectToSimulate.Translation);
				//		Task.Delay(1000).Wait();
				//	}
				//});
				//thread.IsBackground = true;
				//thread.Start();

				//Task.Delay(1).Wait();
			}
			Console.ReadLine();
		}
	}
}