using System;
using System.Linq;

namespace StarterAssets.DelaunayTriangulation
{
	public class Triangle
	{
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Triangle)obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(vertices, edges, circumcentre, circumradius);
		}

		public readonly Point[] vertices;
		public readonly Edge[] edges;
		public readonly Point circumcentre;
		public readonly float circumradius;
		
		public Triangle(Point[] vertices, Edge[] edges)
		{
			this.vertices = vertices;
			this.edges = edges;
			circumcentre = GetCircumcentre();
			circumradius = GetCircumradius();
		}

		public bool ContainsEdge(Edge edge) =>
			edge == edges[0] ||
			edge == edges[1] ||
			edge == edges[2];

		public int ContainsPoint(Triangle other)
		{
			int result = 0;
			if (vertices[0] == other.vertices[0] 
			    || vertices[0] == other.vertices[1]
			    || vertices[0] == other.vertices[2])
				result++;

			if (vertices[1] == other.vertices[0] 
			    || vertices[1] == other.vertices[1]
			    || vertices[1] == other.vertices[2])
				result++;
			if (vertices[2] == other.vertices[0] 
 			    || vertices[2] == other.vertices[1]
 			    || vertices[2] == other.vertices[2])
 				result++;
			return result;
		}

		private Point GetCircumcentre()
		{
			Point a = vertices[0];
			Point b = vertices[1];
			Point c = vertices[2];

			float d = 2 * (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y));
			Point result = new();
			result.x = 1 / d * (
				((a.x * a.x) + (a.y * a.y)) * (b.y - c.y) +
				((b.x * b.x) + (b.y * b.y)) * (c.y - a.y) +
				((c.x * c.x) + (c.y * c.y)) * (a.y - b.y)
				);
			result.y = 1 / d * (
				((a.x * a.x) + (a.y * a.y)) * (c.x - b.x) +
				((b.x * b.x) + (b.y * b.y)) * (a.x - c.x) +
				((c.x * c.x) + (c.y * c.y)) * (b.x - a.x)
			);
			return result;
		}

		private float GetCircumradius() => 
			(float)Math.Sqrt(Math.Pow(vertices[0].x - circumcentre.x, 2) + Math.Pow(vertices[0].y - circumcentre.y, 2));

		public bool IsVertexInsideCircumcircle(Point point)
		{
			float distance = Point.Distance(point, circumcentre);
			return distance < circumradius;
		}

		public override string ToString()
		{
			return $"{vertices[0]}, {vertices[1]}, {vertices[2]}";
		}

		public static bool operator ==(Triangle a, Triangle b)
		{
			if (ReferenceEquals(a, b))
				return true;
			bool aNull = ReferenceEquals(a, null);
			bool bNull = ReferenceEquals(b, null);
			if (aNull && !bNull || bNull && !aNull)
				return false; 
			return a.vertices.OrderBy(vertex => vertex.x).SequenceEqual(b.vertices.OrderBy(vertex => vertex.x));
		}

		public static bool operator !=(Triangle a, Triangle b) => !(a == b);
		private bool Equals(Triangle other)
		{
			return Equals(vertices, other.vertices) && 
			       Equals(edges, other.edges) &&
			       circumcentre.Equals(other.circumcentre) &&
			       circumradius.Equals(other.circumradius);
		}
	}
}