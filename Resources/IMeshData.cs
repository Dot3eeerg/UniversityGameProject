namespace UniversityGameProject.Resources
{
    public interface IMeshData
    {
        public float[]  Vertices { get; }
        public ushort[]  Indices { get; }
        
        public void ApplyScale(float scaleX, float scaleY) { }
    }
}
