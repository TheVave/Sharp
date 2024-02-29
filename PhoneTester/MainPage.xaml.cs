using SharpPhysics._2d.Rendering._2DSGLESRenderer.Windowing;

namespace PhoneTester
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();
		}

		private void Start_Clicked(object sender, EventArgs e)
		{
			Loading.IsVisible = true;
			((Button)sender).IsVisible = true;
			SDLTester.Run();
		}
	}

}
