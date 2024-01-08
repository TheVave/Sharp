using SharpPhysics.Renderer;
using SharpPhysics.Renderer.Textures;


MainRenderer.ExecuteBeforeLoad += Load;
void Load(object? sender, string e)
{
	MainRenderer.Display.objectToRender.OTexture = TexturePreloader.GetFilePreloadedInfo("main");
}

MainRenderer.InitRendering();