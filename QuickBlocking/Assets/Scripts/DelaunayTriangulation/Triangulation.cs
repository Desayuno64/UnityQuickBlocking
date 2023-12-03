using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StarterAssets.DelaunayTriangulation
{
	public class Triangulation
	{
		private readonly Point[] points;
		public List<Triangle> triangulation;
		public Triangle superTriangle;
		private readonly TriangleComponent trianglePrefab;

		public Triangulation(Point[] points, TriangleComponent trianglePrefab)
		{
			this.trianglePrefab = trianglePrefab;
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
				List<Edge> polygon = new();
				Point point = points[i];
				
				for (int j = 0; j < triangulation.Count; j++)
				{
					Triangle triangle = triangulation[j];
					if (triangle.IsVertexInsideCircumcircle(point))
						badTriangles.Add(triangle);
				}

				if (badTriangles.Count == 1)
				{
					polygon.Add(badTriangles[0].edges[0]);
					polygon.Add(badTriangles[0].edges[1]);
					polygon.Add(badTriangles[0].edges[2]);
					Debug.Log($"ADD ONLY BAD {i}");
				}
				else
				{
					for (int j = 0; j < badTriangles.Count; j++)
					{
						Triangle triangle = badTriangles[j];
						for (int k = 0; k < badTriangles.Count; k++)
						{
							if (k == j)
								continue;


							bool equal1 = triangle.edges[0] == badTriangles[k].edges[0]
										  || triangle.edges[0] == badTriangles[k].edges[1]
										  || triangle.edges[0] == badTriangles[k].edges[2];
							if(equal1)
								polygon.Add(triangle.edges[0]);
								
							equal1 = triangle.edges[1] == badTriangles[k].edges[0]
										  || triangle.edges[1] == badTriangles[k].edges[1]
										  || triangle.edges[1] == badTriangles[k].edges[2];
							if(equal1)
								polygon.Add(triangle.edges[1]);
						
							equal1 = triangle.edges[2] == badTriangles[k].edges[0]
										  || triangle.edges[2] == badTriangles[k].edges[1]
										  || triangle.edges[2] == badTriangles[k].edges[2];
							if(equal1)
								polygon.Add(triangle.edges[2]);
							
						}
					}
				}
				
				for (int j = 0; j < triangulation.Count; j++)
				{
					if (badTriangles.Any(t => t == triangulation[j]))
						triangulation[j] = null;
				}
				triangulation = triangulation.Where(triangle => triangle != null).ToList();

				polygon = polygon.Distinct().ToList();
				for (int j = 0; j < polygon.Count; j++)
				{
					Triangle result = CreateFromEdgePoint(polygon[j], point);
					triangulation.Add(result);
					Debug.Log($"ADD FROM POLYGON {i}");
				}
			}

			for (int i = 0; i < triangulation.Count; i++)
			{
				Triangle triangle = triangulation[i];
				int amount = superTriangle.ContainsPoint(triangle);
				if(amount > 1)
					triangulation[i] = null;
			}

			triangulation = triangulation.Where(triangle => triangle != null).ToList();
			Debug.Log($"AMOUNT {triangulation.Count}");
			triangulation.Add(superTriangle);
			for (int i = 0; i < triangulation.Count; i++)
			{
				var component = Object.Instantiate(trianglePrefab);
				component.SetData(triangulation[i].edges, triangulation[i].circumcentre, triangulation[i].circumradius);
			}
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