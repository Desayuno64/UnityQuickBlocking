using System.Linq;
using UnityEngine;

namespace StarterAssets.DelaunayTriangulation
{
	public class TriangulateTestComponent : MonoBehaviour
	{
		[SerializeField] private Transform[] points;


#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (points == null || points.Length == 0)
				return;
			
			Triangulation triangulation = new(points.Select(point => new V2(point.position)).ToArray());
			Triangle superTriangle = triangulation.CreateSuperTriangle();
			Gizmos.color = Color.red;
			Gizmos.DrawLine(superTriangle.edges[0].vertex1.ToVector2(), superTriangle.edges[0].vertex2.ToVector2());
			Gizmos.DrawLine(superTriangle.edges[1].vertex1.ToVector2(), superTriangle.edges[1].vertex2.ToVector2());
			Gizmos.DrawLine(superTriangle.edges[2].vertex1.ToVector2(), superTriangle.edges[2].vertex2.ToVector2());
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(superTriangle.circumcentre.ToVector2(), superTriangle.circumradius);
		}
#endif
	}
}