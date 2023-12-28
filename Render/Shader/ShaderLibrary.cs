namespace UniversityGameProject.Render.Shader;

static class ShaderLibrary
{
    public static ShaderProgram TextureShader(ShaderContext context)
    {
        string vertTextureSource = File.ReadAllText("Render/Shader/ShaderResources/Texture/shaderTexture.frag");
        string fragTextureSource = File.ReadAllText("Render/Shader/ShaderResources/Texture/shaderTexture.vert");

        Shader vert = new Shader(context, ShaderType.VertexShader, vertTextureSource);
        Shader frag = new Shader(context, ShaderType.FragmentShader, fragTextureSource);

        ShaderProgram program = new ShaderProgram(context);
        program.AttachShader(vert);
        program.AttachShader(frag);

        return program;
    }
}