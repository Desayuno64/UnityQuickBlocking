using System;
using System.Linq;

namespace StarterAssets.DelaunayTriangulation
{
	public class Triangulation
	{
		private readonly V2[] points;

		public Triangulation(V2[] points)
		{
			this.points = points;
		}

		public Triangle CreateSuperTriangle()
		{
			V2[] sortedPointsHorizontal = points.OrderBy(point => point.x).ToArray();
			V2[] sortedPointsVertical = points.OrderBy(point => point.x).ToArray();
			float xDistance = sortedPointsHorizontal.Last().x - sortedPointsHorizontal.First().x;
			float yDistance = sortedPointsVertical.Last().y - sortedPointsVertical.First().y;
			float length = Math.Max(xDistance, yDistance);
			length *= 4;
			float halfLength = length / 2;
			V2 center = V2.Average(points);

			V2[] resultVertex =
			{
				new(center.x + halfLength, center.y - halfLength),
				new(center.x, center.y + halfLength),
				new(center.x - halfLength, center.y - halfLength)
			};
			Edge[] resultEdges =
			{
				new(resultVertex[0], resultVertex[1]),
				new(resultVertex[1], resultVertex[2]),
				new(resultVertex[2], resultVertex[0]),
			};

			return new(resultVertex, resultEdges);
		}
	}
}