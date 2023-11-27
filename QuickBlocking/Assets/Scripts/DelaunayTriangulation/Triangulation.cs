using System;
using System.Collections.Generic;
using System.Linq;

namespace StarterAssets.DelaunayTriangulation
{
	public class Triangulation
	{
		private readonly V2[] points;
		public readonly List<Triangle> triangulation;
		private V2[] sortedPoints;
		private List<Triangle> badTriangles;
		public Triangle superTriangle;
		
		public Triangulation(V2[] points)
		{
			badTriangles = new();
			triangulation = new();
			this.points = points;
		}

		public void Calculate()
		{
			superTriangle = CreateSuperTriangle();
			badTriangles.Add(superTriangle);

			List<Triangle> tempTriangles = new();
			for (int i = 0; i < sortedPoints.Length; i++)
			{
				V2 vertex = sortedPoints[i];
				if (i == 0)
				{
					tempTriangles.AddRange(CreateNewTriangles(vertex, superTriangle));
					continue;
				}

				int length = tempTriangles.Count;
				for (int j = 0; j < length; j++)
				{
					if (!tempTriangles[j].IsVertexInsideCircumcircle(vertex))
					{
						Triangle triangle = tempTriangles[j];
						tempTriangles.Remove(triangle);
						tempTriangles.TrimExcess();
						badTriangles.Add(triangle);
					}
					tempTriangles.AddRange(CreateNewTriangles(vertex, tempTriangles[j]));
				}
			}
			triangulation.AddRange(tempTriangles);
			RemoveTrianglesFromSuperTriangle();
		}

		private void RemoveTrianglesFromSuperTriangle()
		{
			List<int> indexesToRemove = new();
			V2 v1 = superTriangle.vertices[0];
			V2 v2 = superTriangle.vertices[1];
			V2 v3 = superTriangle.vertices[2];
			
			for (int i = 0; i < triangulation.Count; i++)
			{
				int count = 0;
				for (int j = 0; j < 3; j++)
				{
					if (triangulation[i].vertices[j] == v1 ||
					    triangulation[i].vertices[j] == v2 ||
					    triangulation[i].vertices[j] == v3)
						count++;
				}			
				
				if(count >= 2)
					indexesToRemove.Add(i);
			}
			
			for(int i = 0; i < indexesToRemove.Count; i++)
				triangulation.RemoveAt(indexesToRemove[i]);
			
			triangulation.TrimExcess();
		}
		private Triangle[] CreateNewTriangles(V2 vertex, Triangle triangle)
		{
			Triangle[] result =
			{
				new(new[]
				{
					vertex, triangle.vertices[0], triangle.vertices[1]
				}, new Edge[]
				{
					new(vertex, triangle.vertices[0]),
					new(vertex, triangle.vertices[1]),
					new(triangle.vertices[0], triangle.vertices[1])
				}),
				new(new[]
				{
					vertex, triangle.vertices[1], triangle.vertices[2]
				}, new Edge[]
				{
					new(vertex, triangle.vertices[1]),
					new(vertex, triangle.vertices[2]),
					new(triangle.vertices[1], triangle.vertices[2])
				}),
				new(new[]
				{
					vertex, triangle.vertices[2], triangle.vertices[0]
				}, new Edge[]
				{
					new(vertex, triangle.vertices[2]),
					new(vertex, triangle.vertices[0]),
					new(triangle.vertices[2], triangle.vertices[0])
				})
			};

			return result;
		}

		public Triangle CreateSuperTriangle()
		{
			V2[] sortedPointsHorizontal = points.OrderBy(point => point.x).ToArray();
			V2[] sortedPointsVertical = points.OrderBy(point => point.x).ToArray();
			sortedPoints = sortedPointsHorizontal;
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