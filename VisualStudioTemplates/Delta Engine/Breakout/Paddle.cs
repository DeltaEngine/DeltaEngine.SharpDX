using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Input;
using DeltaEngine.Rendering2D;

namespace $safeprojectname$
{
	public class Paddle : Sprite
	{
		public Paddle() : base(new Material(Shader.Position2DColorUV, "Paddle"), Rectangle.One)
		{
			RegisterInputCommands();
			Start<RunPaddle>();
			RenderLayer = 5;
		}

		private void RegisterInputCommands()
		{
			new Command(() => 
			{
				xPosition -= PaddleMovementSpeed * Time.Delta;
			}).Add(new KeyTrigger(Key.CursorLeft, State.Pressed));
			new Command(() => 
			{
				xPosition += PaddleMovementSpeed * Time.Delta;
			}).Add(new KeyTrigger(Key.CursorRight, State.Pressed));
			new Command(pos => 
			{
				xPosition += pos.X - Position.X;
			}).Add(new MouseButtonTrigger(MouseButton.Left, State.Pressed));
		}

		private float xPosition = 0.5f;
		private const float PaddleMovementSpeed = 1.5f;
		public class RunPaddle : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					var paddle = (Paddle)entity;
					var xPosition = paddle.xPosition.Clamp(HalfWidth, 1.0f - HalfWidth);
					paddle.xPosition = xPosition;
					paddle.DrawArea = Rectangle.FromCenter(xPosition, YPosition, Width, Height);
				}
			}
		}
		private const float YPosition = 0.9f;
		internal const float HalfWidth = Width / 2.0f;
		private const float Width = 0.2f;
		private const float Height = 0.04f;

		public Vector2D Position
		{
			get
			{
				return new Vector2D(DrawArea.Center.X, DrawArea.Top);
			}
		}
	}
}