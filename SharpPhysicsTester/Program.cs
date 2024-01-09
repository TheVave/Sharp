using SharpPhysics.Renderer;
using SharpPhysics.Renderer.Textures;


MainRenderer.TextureLoader += textureLoad;
void textureLoad(object? sender, EventArgs e)
{
	MainRenderer.Display.objectToRender.OTexture = TexturePreloader.GetFilePreloadedInfo("main");
}

MainRenderer.InitRendering();