using SharpPhysics.Renderer;
using SharpPhysics.Renderer.Textures;



MainRenderer.TextureLoader += MainLoadTextures;

void MainLoadTextures(object? sender, RenderedObject e)
{
	e.OTexture = TexturePreloader.GetFilePreloadedInfo("main");
};



MainRenderer.InitRendering();