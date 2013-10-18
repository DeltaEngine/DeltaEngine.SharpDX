using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering2D.Particles
{
	public abstract class HierarchicalObject
	{
		protected HierarchicalObject()
		{
			children = new List<HierarchicalObject>();
			Rotation = Quaternion.Identity;
		}

		protected HierarchicalObject(Vector3D position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}

		public Vector3D Position
		{
			get { return position; }
			set
			{
				position = value;
				foreach (var child in Children)
					child.UpdateAbsolutePositionAndRotationFromParent();
				OnPositionChange();
			}
		}

		private Vector3D position;

		protected virtual void OnPositionChange() {}

		public Quaternion Rotation
		{
			get { return rotation; }
			set
			{
				rotation = value;
				foreach (var child in Children)
					child.UpdateAbsolutePositionAndRotationFromParent();
				OnRotationChange();
			}
		}

		private Quaternion rotation;

		protected virtual void OnRotationChange() { }

		public virtual Vector3D PositionRelativeToParent { get; set; }
		public HierarchicalObject Parent { get; set; }
		public List<HierarchicalObject> Children { get { return children; } }

		private readonly List<HierarchicalObject> children; 

		public void AddChild(HierarchicalObject child)
		{
			Children.Add(child);
			child.Parent = this;
			child.UpdateAbsolutePositionAndRotationFromParent();
		}

		public void UpdateAbsolutePositionAndRotationFromParent()
		{
			if (Parent == null)
				Position = PositionRelativeToParent;
			else
			{
				Position = Parent.Position + PositionRelativeToParent.Transform(Parent.Rotation);
				Rotation = Parent.Rotation;
			}
			foreach (var child in Children)
				child.UpdateAbsolutePositionAndRotationFromParent();
		}
	}
}