using System;
using UnityEngine;

namespace StarterAssets.DelaunayTriangulation
{
	public struct V2
	{
		private const float TOLERANCE = 0.01f;
		
		public float x;
		public float y;
		
		public V2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public V2(Vector2 vector) : this(vector.x, vector.y) { }
		public readonly Vector2 ToVector2() => new(x, y);

		public static V2 Average(V2[] points)
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

		public static bool operator ==(V2 a, V2 b) => 
			Mathf.Abs(a.x - b.x) < TOLERANCE && Math.Abs(a.y - b.y) < TOLERANCE;
		public static bool operator !=(V2 a, V2 b) => !(a == b);

		private bool Equals(V2 other) => x.Equals(other.x) && y.Equals(other.y);
		public override bool Equals(object obj) => obj is V2 other && Equals(other);
		public override int GetHashCode() => HashCode.Combine(x, y);
	}
}