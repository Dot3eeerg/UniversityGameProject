namespace UniversityGameProject.Render.Shader;

static class ShaderLibrary
{
    public static ShaderProgram TextureShader(ShaderContext context)
    {
        string fragTextureSource = File.ReadAllText("Render/Shader/ShaderResources/Texture/shaderTexture.frag");
        string vertTextureSource = File.ReadAllText("Render/Shader/ShaderResources/Texture/shaderTexture.vert");

        Shader frag = new Shader(context, ShaderType.FragmentShader, fragTextureSource);
        Shader vert = new Shader(context, ShaderType.VertexShader, vertTextureSource);

        ShaderProgram program = new ShaderProgram(context);
        program.AttachShader(frag);
        program.AttachShader(vert);

        return program;
    }
    
    public static ShaderProgram GroundShader(ShaderContext context)
    {
        string fragTextureSource = File.ReadAllText("Render/Shader/ShaderResources/Ground/shaderGround.frag");
        string vertTextureSource = File.ReadAllText("Render/Shader/ShaderResources/Ground/shaderGround.vert");

        Shader frag = new Shader(context, ShaderType.FragmentShader, fragTextureSource);
        Shader vert = new Shader(context, ShaderType.VertexShader, vertTextureSource);

        ShaderProgram program = new ShaderProgram(context);
        program.AttachShader(frag);
        program.AttachShader(vert);

        return program;
    }
}