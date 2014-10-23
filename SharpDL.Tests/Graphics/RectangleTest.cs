using NUnit.Framework;
using SharpDL.Graphics;

namespace SharpDL.Tests.Graphics
{
	[TestFixture]
	public class RectangleTest
	{
		[Test]
		public void Constructor()
		{
			Rectangle rect;
			Assert.AreEqual(0, rect.X);
			Assert.AreEqual(0, rect.Y);
			Assert.AreEqual(0, rect.Width);
			Assert.AreEqual(0, rect.Height);
			Assert.AreEqual(0, rect.Left);
			Assert.AreEqual(0, rect.Right);
			Assert.AreEqual(0, rect.Bottom);
			Assert.AreEqual(0, rect.Top);

			rect = new Rectangle(1, 2, 3, 4);
			Assert.AreEqual(1, rect.X);
			Assert.AreEqual(2, rect.Y);
			Assert.AreEqual(3, rect.Width);
			Assert.AreEqual(4, rect.Height);
			Assert.AreEqual(1, rect.Left);
			Assert.AreEqual(4, rect.Right);
			Assert.AreEqual(6, rect.Bottom);
			Assert.AreEqual(2, rect.Top);

		}

		[Test]
		public void Center()
		{
			var center = new Rectangle(0, 0, 50, 20).Center;

			Assert.AreEqual(25, center.X);
			Assert.AreEqual(10, center.Y);

			center = new Rectangle().Center;

			Assert.AreEqual(0, center.X);
			Assert.AreEqual(0, center.Y);
		}

		[Test]
		public void Intersects()
		{
			var rectA = new Rectangle(0, 0, 50, 50);
			var rectB = new Rectangle(25, 25, 50, 50);

			Assert.IsTrue(rectA.Intersects(rectB));
			Assert.IsTrue(rectB.Intersects(rectA));

			rectB = new Rectangle(51, 0, 50, 50);

			Assert.IsFalse(rectA.Intersects(rectB));
			Assert.IsFalse(rectB.Intersects(rectA));

			rectB = new Rectangle(0, 51, 50, 50);

			Assert.IsFalse(rectA.Intersects(rectB));
			Assert.IsFalse(rectB.Intersects(rectA));

			rectB = new Rectangle(51, 51, 50, 50);

			Assert.IsFalse(rectA.Intersects(rectB));
			Assert.IsFalse(rectB.Intersects(rectA));
		}

		[Test]
		public void ContainsRect()
		{
			var rect = new Rectangle(0, 0, 50, 50);
			Assert.IsFalse(rect.Contains(new Rectangle(25, 25, 50, 50)));

			Assert.IsTrue(rect.Contains(new Rectangle(00, 00, 50, 50)));
			Assert.IsTrue(rect.Contains(new Rectangle(15, 15, 10, 10)));
		}

		[Test]
		public void ContainsPoint()
		{
			var rect = new Rectangle(0, 0, 50, 50);

			Assert.IsTrue(rect.Contains(new Point(00, 00)));
			Assert.IsTrue(rect.Contains(new Point(00, 50)));
			Assert.IsTrue(rect.Contains(new Point(50, 00)));
			Assert.IsTrue(rect.Contains(new Point(50, 50)));

			Assert.IsFalse(rect.Contains(new Point(-1, 00)));
			Assert.IsFalse(rect.Contains(new Point(00, -1)));
			Assert.IsFalse(rect.Contains(new Point(-1, -1)));

			Assert.IsFalse(rect.Contains(new Point(51, 00)));
			Assert.IsFalse(rect.Contains(new Point(00, 51)));
			Assert.IsFalse(rect.Contains(new Point(51, 51)));
		}

		[Test]
		public void ContainsVector()
		{
			var rect = new Rectangle(0, 0, 50, 50);

			Assert.IsTrue(rect.Contains(new Vector(00, 00)));
			Assert.IsTrue(rect.Contains(new Vector(00, 50)));
			Assert.IsTrue(rect.Contains(new Vector(50, 00)));
			Assert.IsTrue(rect.Contains(new Vector(50, 50)));

			Assert.IsFalse(rect.Contains(new Vector(-1, 00)));
			Assert.IsFalse(rect.Contains(new Vector(00, -1)));
			Assert.IsFalse(rect.Contains(new Vector(-1, -1)));

			Assert.IsFalse(rect.Contains(new Vector(51, 00)));
			Assert.IsFalse(rect.Contains(new Vector(00, 51)));
			Assert.IsFalse(rect.Contains(new Vector(51, 51)));
		}

		[Test]
		public void Intercet()
		{
			var rectA = new Rectangle(0, 0, 50, 50);
			var rectB = new Rectangle(0, 0, 50, 50);

			var result = Rectangle.Intersect(rectA, rectB);

			Assert.AreEqual(00, result.X);
			Assert.AreEqual(00, result.Y);
			Assert.AreEqual(50, result.Width);
			Assert.AreEqual(50, result.Height);

			rectA = new Rectangle(0, 0, 50, 50);
			rectB = new Rectangle(25, 25, 50, 50);

			result = Rectangle.Intersect(rectA, rectB);

			Assert.AreEqual(25, result.X);
			Assert.AreEqual(25, result.Y);
			Assert.AreEqual(25, result.Width);
			Assert.AreEqual(25, result.Height);

			rectA = new Rectangle(0, 0, 50, 50);
			rectB = new Rectangle(50, 50, 50, 50);

			result = Rectangle.Intersect(rectA, rectB);

			Assert.AreEqual(50, result.X);
			Assert.AreEqual(50, result.Y);
			Assert.AreEqual(0, result.Width);
			Assert.AreEqual(0, result.Height);

			rectA = new Rectangle(0, 0, 50, 50);
			rectB = new Rectangle(51, 51, 50, 50);

			result = Rectangle.Intersect(rectA, rectB);

			Assert.AreEqual(0, result.X);
			Assert.AreEqual(0, result.Y);
			Assert.AreEqual(0, result.Width);
			Assert.AreEqual(0, result.Height);
		}
	}
}

