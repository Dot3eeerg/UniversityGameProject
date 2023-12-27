namespace UniversityGameProject.Main.Scene;

public abstract class MainLoop
{
    public MainLoop() { }
    
    protected virtual void Process(float delta) { }
}