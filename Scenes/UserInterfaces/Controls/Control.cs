using System;
using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D;

namespace DeltaEngine.Scenes.UserInterfaces.Controls
{
	/// <summary>
	/// The basis for most UI controls that can respond to mouse or touch input. Although it is a Sprite
	/// it defaults to a transparent material as a Control. Can also be used as a container (e.g. Panel)
	/// </summary>
	public class Control : Sprite, Updateable
	{
		protected Control() {}

		protected Control(Rectangle drawArea)
			: base(Material.EmptyTransparentMaterial, drawArea)
		{
			Add(new InteractiveState());
			Start<ControlUpdater>();
		}

		protected void AddChild(Entity2D entity)
		{
			if (children.Any(c => c.Entity2D == entity))
				return;
			children.Add(new Child(entity));
			entity.RenderLayer = RenderLayer + children.Count;
		}

		private readonly List<Child> children = new List<Child>();

		protected class Child
		{
			public Child(Entity2D control)
			{
				Entity2D = control;
				Control = control as Control;
				WasEnabled = true;
				WasActive = true;
				WasVisible = true;
			}

			public Entity2D Entity2D { get; private set; }
			public Control Control { get; private set; }
			public bool WasEnabled { get; set; }
			public bool WasActive { get; set; }
			public bool WasVisible { get; set; }
		}

		protected void RemoveChild(Entity2D entity)
		{
			var child = children.FirstOrDefault(c => c.Entity2D == entity);
			if (child != null)
				children.Remove(child);
		}

		public virtual bool IsEnabled
		{
			get { return isEnabled; }
			set
			{
				if (isEnabled == value)
					return;
				isEnabled = value;
				if (isEnabled)
					EnableControl();
				else
					DisableControl();
			}
		}
		private bool isEnabled = true;

		private void EnableControl()
		{
			foreach (Child child in children.Where(c => c.Control != null))
				child.Control.IsEnabled = child.WasEnabled;
		}

		private void DisableControl()
		{
			foreach (Child child in children.Where(c => c.Control != null))
			{
				child.WasEnabled = child.Control.IsEnabled;
				child.Control.IsEnabled = false;
			}
		}

		public override bool IsActive
		{
			get { return base.IsActive; }
			set
			{
				if (base.IsActive == value)
					return;
				base.IsActive = value;
				if (value)
					ActivateChildControls();
				else
					InactivateChildControls();
			}
		}

		private void ActivateChildControls()
		{
			foreach (Child child in children)
				child.Entity2D.IsActive = child.WasActive;
		}

		private void InactivateChildControls()
		{
			foreach (Child child in children)
			{
				child.WasActive = child.Entity2D.IsActive;
				child.Entity2D.IsActive = false;
			}
		}

		public override void ToggleVisibility()
		{
			base.ToggleVisibility();
			if (IsVisible)
				ShowChildControls();
			else
				HideChildControls();
		}

		private void ShowChildControls()
		{
			foreach (Child child in children)
				child.Entity2D.IsVisible = child.WasVisible;
		}

		private void HideChildControls()
		{
			foreach (Child child in children)
			{
				child.WasVisible = child.Entity2D.IsVisible;
				child.Entity2D.IsVisible = false;
			}
		}

		public virtual void Update()
		{
			for (int i = 0; i < children.Count; i++)
				children[i].Entity2D.RenderLayer = RenderLayer + i + 1;
		}

		public bool IsPauseable { get { return false; } }

		public InteractiveState State
		{
			get { return Get<InteractiveState>(); }
		}

		public virtual void Click()
		{
			if (Clicked != null)
				Clicked();
		}

		public Action Clicked;
	}
}