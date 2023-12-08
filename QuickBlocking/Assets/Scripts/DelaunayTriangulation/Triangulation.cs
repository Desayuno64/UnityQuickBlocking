using System;
using System.Collections.Generic;
using System.Linq;

namespace StarterAssets.DelaunayTriangulation
{
	public class Triangulation
	{
		private readonly Point[] points;
		public List<Triangle> triangulation;
		public Triangle superTriangle;

		public Triangulation(Point[] points)
		{
			triangulation = new();
			this.points = points;
		}

		public void Calculate()
		{
			superTriangle = CreateSuperTriangle();
			triangulation.Add(superTriangle);

			for (int i = 0; i < points.Length; i++)
			{
				List<Triangle> badTriangles = new();
				Point point = points[i];

				foreach (Triangle triangle in triangulation)
				{
					if (triangle.IsVertexInsideCircumcircle(point))
						badTriangles.Add(triangle);
				}

				List<Edge> polygon = new();
				for (int j = 0; j < badTriangles.Count; j++)
				{
					for (int k = 0; k < 3; k++)
					{
						bool addToPolygon = false;

						for (int h = 0; h < badTriangles.Count; h++)
						{
							if (h != j && badTriangles[h].ContainsEdge(badTriangles[j].edges[k]))
							{
								addToPolygon = true;
							}
						}
						
						if(!addToPolygon)
							polygon.Add(badTriangles[j].edges[k]);
					}
				}

				for (int j = 0; j < triangulation.Count; j++)
				{
					if (badTriangles.Any(t => t == triangulation[j]))
						triangulation[j] = null;
				}
				triangulation = triangulation.Where(t => t != null).ToList();

				for (int j = 0; j < polygon.Count; j++)
				{
					Triangle t = CreateFromEdgePoint(polygon[j], point);
					triangulation.Add(t);
				}
			}
			
			for (int i = 0; i < triangulation.Count; i++)
			{
				Triangle triangle = triangulation[i];
				int amount = superTriangle.ContainsPoint(triangle);
				if(amount >= 1)
					triangulation[i] = null;
			}

			triangulation = triangulation.Where(triangle => triangle != null).ToList();
		}

		private Triangle CreateFromEdgePoint(Edge edge, Point point) =>
			new(new[]
				{
					edge.vertex1, edge.vertex2, point
				},
				new[]
				{
					edge, new(edge.vertex1, point), new(edge.vertex2, point)
				});

		public Triangle CreateSuperTriangle()
		{
			Point[] sortedPointsHorizontal = points.OrderBy(point => point.x).ToArray();
			Point[] sortedPointsVertical = points.OrderBy(point => point.x).ToArray();
			float xDistance = sortedPointsHorizontal.Last().x - sortedPointsHorizontal.First().x;
			float yDistance = sortedPointsVertical.Last().y - sortedPointsVertical.First().y;
			float length = Math.Max(xDistance, yDistance);
			length *= 4;
			float halfLength = length / 2;
			Point center = Point.Average(points);

			Point[] resultVertex =
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