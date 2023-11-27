using System;

namespace StarterAssets.DelaunayTriangulation
{
	public record Triangle
	{
		public readonly V2[] vertices;
		public readonly Edge[] edges;
		public readonly V2 circumcentre;
		public readonly float circumradius;
		
		public Triangle(V2[] vertices, Edge[] edges)
		{
			this.vertices = vertices;
			this.edges = edges;
			circumcentre = GetCircumcentre();
			circumradius = GetCircumradius();
		}

		private V2 GetCircumcentre()
		{
			V2 a = vertices[0];
			V2 b = vertices[1];
			V2 c = vertices[2];

			float d = 2 * (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y));
			V2 result = new();
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

		public bool IsVertexInsideCircumcircle(V2 vertex)
		{
			float distance = V2.Distance(vertex, circumcentre);
			return distance < circumradius;
		}
	}
}