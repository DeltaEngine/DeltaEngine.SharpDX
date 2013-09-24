using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Particles
{
	public interface Particle
	{
		float Rotation { get; set; }
		float ElapsedTime { get; set; }
		Size Size { get; set; }
		Color Color { get; set; }
		bool IsActive { get; set; }
		int CurrentFrame { get; set; }
		Rectangle CurrentUV { get; set; }
	}
}