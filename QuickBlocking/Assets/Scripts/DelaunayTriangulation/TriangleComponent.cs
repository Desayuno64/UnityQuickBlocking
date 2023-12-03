using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StarterAssets.DelaunayTriangulation
{
	public class TriangleComponent : MonoBehaviour
	{
		public Color gizmoColour;
		private Vector2 circumcentre;
		private float circumradius;
		private Edge[] edges;

		public void SetData(Edge[] edges, Point circumcentre, float circumradius)
		{
			float value = Random.Range(0f, 1f);
			gizmoColour = new(value, value, value, 1f);
			this.edges = edges;
			this.circumradius = circumradius;
			this.circumcentre = circumcentre.ToVector2();
		}
		
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = gizmoColour;
			Gizmos.DrawWireSphere(circumcentre, circumradius);
			for (int i = 0; i < edges.Length; i++)
			{
				Gizmos.DrawLine(edges[i].vertex1.ToVector2(), edges[i].vertex2.ToVector2());
			}
		}
	}
}