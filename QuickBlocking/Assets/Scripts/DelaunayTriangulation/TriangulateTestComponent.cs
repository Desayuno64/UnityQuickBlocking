using System.Linq;
using UnityEngine;

namespace StarterAssets.DelaunayTriangulation
{
	public class TriangulateTestComponent : MonoBehaviour
	{
		[SerializeField] private TriangleComponent trianglePrefab;
		[SerializeField] private Transform[] points;
		private Triangulation delaunay; 

		[ContextMenu(nameof(Calculate))]
		public void Calculate()
		{
			delaunay = new(points.Select(point => new Point(point.position)).ToArray(), trianglePrefab);
			delaunay.Calculate();
		}
	}
}
