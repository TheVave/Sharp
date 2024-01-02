All of the parameters defining how the object should be simulated.
Values:
	Simulate physics, you can still include this object in collidable arrays
    public bool SimulatePhysics = true;

    the gravity multiplier, 1 to do normal and 0 for none, 9.8 dist/s
    public int GravityMultiplier = 1;

    Increases the accuracy of the physics simulation but decreases the porformance
    public int TicksPerSecond = 20;

    The objects that the object to simulate can collide with
    public _2dSimulatedObject[] CollidableObjects = Array.Empty<_2dSimulatedObject>();

    The objects that the object is linked with, so that it will follow the motion of the parent.
    public _2dSimulatedObject[] LinkedObjects = Array.Empty<_2dSimulatedObject>();

    the time multiplier
    public double TimeMultiplier = 1;

    Mass of the object
    public double Mass = 1f;

    WARNING: unused
    public double SpeedResistance = 0.1;

    The acceleration of the object in SpeedDirection. 
    Not recommended to be used, may have strange side effects.
    public double SpeedAcceleration = 0;

    the rotation resistance of the object
    public double RotResistance = 0.05;

    Speed in SpeedDirection
    public double Speed = 1;

    3d direction using double[2]
    eg. SpeedDirection = new double { 10,0 }
    the object would go ten units in the x direction and 10 in the y direction
    public double[] Acceleration = new double[] { 0, 0 };

    momentum
    public double[] Momentum = new double[] { 0, 0 };

    The acceleration that the object will be experiencing.
    public double RotationalAcceleration = 0;

    The rotational momentum of the object
    Treated very similar to regular momentum.
    public double RotationalMomentum = 0;
No methods.