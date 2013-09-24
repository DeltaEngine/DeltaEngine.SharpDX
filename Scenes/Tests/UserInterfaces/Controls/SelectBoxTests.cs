using System.Collections.Generic;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class SelectBoxTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			selectBox = new SelectBox(Center, Values);
		}

		private SelectBox selectBox;

		private static readonly Rectangle Center = new Rectangle(0.4f, 0.3f, 0.2f, 0.05f);
		private static readonly List<object> Values = new List<object>
		{
			"value 1",
			"value 2",
			"value 3"
		};

		[Test]
		public void RenderSelectBoxWithThreeValuesAndThreeLines()
		{
			var text = new FontText(Font.Default, "", new Rectangle(0.4f, 0.7f, 0.2f, 0.1f));
			selectBox.LineClicked += lineNo => text.Text = selectBox.Values[lineNo] + " clicked";
		}

		[Test]
		public void RenderSelectBoxWithTenValuesAndThreeLines()
		{
			selectBox.Values = new List<object>
			{
				"value 1",
				"value 2",
				"value 3",
				"value 4",
				"value 5",
				"value 6",
				"value 7",
				"value 8",
				"value 9",
				"value 10"
			};
			var text = new FontText(Font.Default, "", new Rectangle(0.4f, 0.7f, 0.2f, 0.1f));
			selectBox.LineClicked += lineNo => text.Text = selectBox.Values[lineNo] + " clicked";
		}

		[Test]
		public void RenderGrowingSelectBox()
		{
			selectBox.Values = new List<object> { "value 1", "value 2", "value 3", "value 4" };
			selectBox.Start<Grow>();
			var text = new FontText(Font.Default, "", new Rectangle(0.4f, 0.7f, 0.2f, 0.1f));
			selectBox.LineClicked += lineNo => text.Text = selectBox.Values[lineNo] + " clicked";
		}

		//ncrunch: no coverage start
		private class Grow : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (SelectBox selectBox in entities)
				{
					var center = selectBox.DrawArea.Center + new Vector2D(0.01f, 0.01f) * Time.Delta;
					var size = selectBox.DrawArea.Size * (1.0f + Time.Delta / 10);
					selectBox.DrawArea = Rectangle.FromCenter(center, size);
				}
			}
		}

		//ncrunch: no coverage end

		[Test]
		public void RenderSelectBoxAttachedToMouse()
		{
			selectBox.Values = new List<object> { "value 1", "value 2", "value 3", "value 4" };
			new Command(
				point => selectBox.DrawArea = Rectangle.FromCenter(point, selectBox.DrawArea.Size)).Add(
					new MouseMovementTrigger());
		}

		[Test]
		public void SetValuesAsNull()
		{			
			Assert.Throws<SelectBox.MustBeAtLeastOneValue>(() => selectBox.Values = null);
		}
	}
}