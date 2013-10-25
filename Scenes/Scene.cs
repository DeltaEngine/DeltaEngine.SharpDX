using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering2D;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using DeltaEngine.ScreenSpaces;

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
			: base("<GeneratedScene>") {}

		public void Add(IEnumerable<Entity2D> newControls)
		{
			foreach (Entity2D control in newControls)
				Add(control);
		}

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
			{
				control.IsVisible = true;
				control.IsActive = true;
			}
			isShown = true;
		}

		public void Hide()
		{
			foreach (var control in controls)
			{
				control.IsVisible = false;
				control.IsActive = false;
			}
			isShown = false;
		}

		public void ToBackground()
		{
			foreach (Control control in controls.OfType<Control>())
				control.Stop<ControlUpdater>();
		}

		public void ToForeground()
		{
			foreach (Control control in controls.OfType<Control>())
				control.Start<ControlUpdater>();
		}

		public virtual void Clear()
		{
			foreach (Entity2D control in controls)
				control.IsActive = false;
			controls.Clear();
		}

		public void SetQuadraticBackground(string imageName)
		{
			SetQuadraticBackground(new Material(Shader.Position2DColorUV, imageName));
		}

		public void SetQuadraticBackground(Material material)
		{
			if (background != null)
				Remove(background);
			background = new Sprite(material, Rectangle.One) { RenderLayer = int.MinValue };
			Add(background);
		}

		protected Sprite background;

		public void SetViewportBackground(string imageName)
		{
			SetViewportBackground(new Material(Shader.Position2DColorUV, imageName));
		}

		public void SetViewportBackground(Material material)
		{
			if (background != null)
				Remove(background);
			var screen = ScreenSpace.Current;
			background = new Sprite(material, screen.Viewport) { RenderLayer = int.MinValue };
			ScreenSpace.Current.ViewportSizeChanged += () => 
				background.SetWithoutInterpolation(screen.Viewport);
			Add(background);
		}

		protected override void DisposeData() {}

		protected override void LoadData(Stream fileData)
		{
			var sceneData = (Scene)new BinaryReader(fileData).Create();
			controls = sceneData.Controls;
			foreach (var control in controls)
			{
				if (control.GetType() == typeof(Label) || control.GetType() == typeof(Button))
					(control as Label).SetFontText();
			}
			background = sceneData.background;
		}
	}
}