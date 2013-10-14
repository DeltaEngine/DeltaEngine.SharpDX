using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering2D;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace DeltaEngine.Scenes.UserInterfaces.EntityDebugger
{
	/// <summary>
	/// The app does not need to be paused to use EntityWriter but it likely makes sense to do so: 
	/// Every frame the contents of the controls are written back to the Entity. If invoked through 
	/// EntitySelector the app will be paused. Components can be edited but not added or removed.
	/// </summary>
	public class EntityWriter : EntityEditor
	{
		public EntityWriter(Entity entity)
			: base(entity) {}

		public override void Update()
		{
			var entity2D = Entity as Entity2D;
			if (entity2D != null && entity2D.RotationCenter.IsNearlyEqual(entity2D.DrawArea.Center))
				KeepRotationCenterInLineWithDrawAreaCenter(entity2D);
			foreach (var pair in componentControls)
				UpdateComponentFromControls(pair.Key, pair.Value);
		}

		private void KeepRotationCenterInLineWithDrawAreaCenter(Entity2D entity2D)
		{
			List<Control> drawAreaControls;
			List<Control> rotationCenterControls;
			if (componentControls.TryGetValue(typeof(Rectangle), out drawAreaControls))
				if (componentControls.TryGetValue(typeof(Vector2D), out rotationCenterControls))
					UpdateRotationCenterIfDrawAreaHasChanged(entity2D, ((TextBox)drawAreaControls[1]).Text,
						(TextBox)rotationCenterControls[1]);
		}

		private static void UpdateRotationCenterIfDrawAreaHasChanged(Entity2D entity2D,
			string drawAreaText, TextBox rotationCenterControl)
		{
			var drawArea = (Rectangle)GetComponentFromString(typeof(Rectangle), drawAreaText);
			if (entity2D.DrawArea != drawArea)
				rotationCenterControl.Text = drawArea.Center.ToString();
		}

		private static object GetComponentFromString(Type componentType, string text)
		{
			try
			{
				return Activator.CreateInstance(componentType, new object[] { text });
			}
			catch (Exception)
			{
				return Activator.CreateInstance(componentType);
			}
		}

		private void UpdateComponentFromControls(Type componentType, List<Control> controls)
		{
			if (componentType == typeof(Color))
				UpdateColorFromSliders(controls);
			else
				UpdateComponentFromTextBox(componentType, (TextBox)controls[1]);
		}

		private void UpdateColorFromSliders(List<Control> controls)
		{
			var r = (byte)((Slider)controls[1]).Value;
			var g = (byte)((Slider)controls[3]).Value;
			var b = (byte)((Slider)controls[5]).Value;
			var a = (byte)((Slider)controls[7]).Value;
			var color = new Color(r, g, b, a);
			if (Entity.Get<Color>() != color)
				Entity.Set(color);
		}

		private void UpdateComponentFromTextBox(Type componentType, TextBox textbox)
		{
			if (textbox.Text != textbox.PreviousText)
				if (componentType == typeof(float))
					TryUpdateFloatIfValueHasChanged(textbox.Text);
				else
					UpdateComponentIfValueHasChanged(componentType, textbox);
		}

		private void TryUpdateFloatIfValueHasChanged(string text)
		{
			try
			{
				UpdateFloatIfValueHasChanged(text);
			}
			catch (Exception)
			{
				SetFloatToZero();
			}
		}

		private void UpdateFloatIfValueHasChanged(string text)
		{
			var value = text.Convert<float>();
			if (Entity.Get<float>() != value)
				Entity.Set(value);
		}

		private void SetFloatToZero()
		{
			if (Entity.Get<float>() != 0.0f)
				Entity.Set(0.0f);
		}

		private void UpdateComponentIfValueHasChanged(Type componentType, TextBox textbox)
		{
			object newComponent = GetComponentFromString(componentType, textbox.Text);
			List<object> oldComponents = Entity.GetComponentsForViewing();
			foreach (var oldComponent in oldComponents)
				if (oldComponent.GetType() == componentType &&
					oldComponent.ToString() != newComponent.ToString())
					Entity.Set(newComponent);
		}
	}
}