using SharpPhysics.Renderer;
using SharpPhysics.Renderer.Textures;


MainRenderer.ExecuteAfterLoad += afterLoad;
void afterLoad(object? sender, EventArgs e)
{
	MainRenderer.Display.objectToRender.OTexture = TexturePreloader.GetFilePreloadedInfo("main");
}

MainRenderer.InitRendering();