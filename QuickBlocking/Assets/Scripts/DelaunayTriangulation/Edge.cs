using System;

namespace StarterAssets.DelaunayTriangulation
{
	public readonly struct Edge
	{
		public readonly Point vertex1;
		public readonly Point vertex2;

		public Edge(Point vertex1, Point vertex2)
		{
			this.vertex1 = vertex1;
			this.vertex2 = vertex2;
		}

		public static bool operator ==(Edge a, Edge b) => a.vertex1 == b.vertex1 && a.vertex2 == b.vertex2;
		public static bool operator !=(Edge a, Edge b) => !(a == b);
		
		private bool Equals(Edge other) => vertex1.Equals(other.vertex1) && vertex2.Equals(other.vertex2);
		public override bool Equals(object obj) => obj is Edge other && Equals(other);
		public override int GetHashCode() => HashCode.Combine(vertex1, vertex2);
	}
}