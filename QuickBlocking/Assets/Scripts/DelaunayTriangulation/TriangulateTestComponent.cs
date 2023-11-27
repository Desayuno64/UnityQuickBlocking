using System.Linq;
using UnityEngine;

namespace StarterAssets.DelaunayTriangulation
{
	public class TriangulateTestComponent : MonoBehaviour
	{
		[SerializeField] private Transform[] points;
		private Triangulation delaunay; 

		[ContextMenu(nameof(Calculate))]
		public void Calculate()
		{
			delaunay = new(points.Select(point => new V2(point.position)).ToArray());
			delaunay.Calculate();
		}
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (delaunay == null || points == null || points.Length == 0)
				return;
			
			delaunay.superTriangle.DrawGizmo(Color.red);
			for (int i = 0; i < delaunay.triangulation.Count; i++)
			{
				delaunay.triangulation[i].DrawGizmo(Color.blue);
			}
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(delaunay.superTriangle.circumcentre.ToVector2(), delaunay.superTriangle.circumradius);
		}
#endif
	}
}