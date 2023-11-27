using UnityEngine;

namespace StarterAssets.DelaunayTriangulation
{
	public static class TriangleExtensions
	{
		public static void DrawGizmo(this Triangle triangle, Color colour)
		{
			Gizmos.color = colour;
			Gizmos.DrawLine(triangle.edges[0].vertex1.ToVector2(), triangle.edges[0].vertex2.ToVector2());
			Gizmos.DrawLine(triangle.edges[1].vertex1.ToVector2(), triangle.edges[1].vertex2.ToVector2());
			Gizmos.DrawLine(triangle.edges[2].vertex1.ToVector2(), triangle.edges[2].vertex2.ToVector2());
		}
	}
}