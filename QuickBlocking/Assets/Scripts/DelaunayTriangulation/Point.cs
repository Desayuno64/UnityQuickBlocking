using System;
using UnityEngine;

namespace StarterAssets.DelaunayTriangulation
{
	public struct Point
	{
		private const float TOLERANCE = 0.01f;
		
		public float x;
		public float y;
		
		public Point(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public Point(Vector2 vector) : this(vector.x, vector.y) { }
		public readonly Vector2 ToVector2() => new(x, y);

		public static Point Average(Point[] points)
		{
			float amount = (float)points.Length;
			float sumX = 0;
			float sumY = 0;
			for (int i = 0; i < amount; i++)
			{
				sumX += points[i].x;
				sumY += points[i].y;
			}

			sumX /= amount;
			sumY /= amount;
			return new(sumX, sumY);
		}

		public static bool operator ==(Point a, Point b) => 
			Mathf.Abs(a.x - b.x) < TOLERANCE && Math.Abs(a.y - b.y) < TOLERANCE;
		public static bool operator !=(Point a, Point b) => !(a == b);
		private bool Equals(Point other) => x.Equals(other.x) && y.Equals(other.y);
		public override bool Equals(object obj) => obj is Point other && Equals(other);
		public override int GetHashCode() => HashCode.Combine(x, y);

		public static float Distance(Point left, Point right) => 
			(float)Math.Sqrt(Math.Pow(left.x - right.x, 2) + Math.Pow(left.y - right.y, 2));

		public override string ToString() => $"{x}, {y}";
	}
}