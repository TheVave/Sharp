using SharpPhysics;
using SharpPhysics._2d._2DSGLESRenderer.Main;

namespace AndroidTester
{
	[Activity(Label = "@string/app_name", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(_Microsoft.Android.Resource.Designer.ResourceConstant.Layout.activity_main);

			MainRendererSGLES.InitRendering();
		}
	}
}