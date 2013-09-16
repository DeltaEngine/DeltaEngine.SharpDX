using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering;
using DeltaEngine.Rendering.Sprites;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace DeltaEngine.Scenes
{
	/// <summary>
	/// Groups Entities such that they can be activated and deactivated together. 
	/// </summary>
	public class Scene : ContentData
	{
		protected Scene(string contentName)
			: base(contentName) {}

		public Scene()
			: base("<GeneratedScene>") { }

		public virtual void Add(Entity2D control)
		{
			if (!controls.Contains(control))
				controls.Add(control);
			control.IsActive = isShown;
		}

		private List<Entity2D> controls = new List<Entity2D>();
		private bool isShown = true;

		public List<Entity2D> Controls
		{
			get { return controls; }
		}

		public void Remove(Entity2D control)
		{
			controls.Remove(control);
			control.IsActive = false;
		}

		public void Show()
		{
			foreach (var control in controls)
				control.IsActive = true;
			isShown = true;
		}

		public void Hide()
		{
			foreach (var control in controls)
				control.IsActive = false;
			isShown = false;
		}

		public void ToBackground()
		{
			foreach (Control control in controls.OfType<Control>())
				control.Stop<Interact>();
		}

		public void ToForeground()
		{
			foreach (Control control in controls.OfType<Control>())
				control.Start<Interact>();
		}

		public virtual void Clear()
		{
			foreach (Entity control in controls)
				control.IsActive = false;
			controls.Clear();
		}

		public void SetBackground(string imageName)
		{
			SetBackground(new Material(Shader.Position2DColorUv, imageName));
		}

		public void SetBackground(Material material)
		{
			if (background != null)
				Remove(background);
			background = new Sprite(material, Rectangle.One) { RenderLayer = int.MinValue };
			Add(background);
		}

		protected Sprite background;

		protected override void DisposeData(){}

		protected override void LoadData(Stream fileData)
		{
			var sceneData = (Scene)new BinaryReader(fileData).Create();
			controls = sceneData.Controls;
			background = sceneData.background;
		}


	}
}