#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteAlways]
public class SplineTriangulator : MonoBehaviour
{
    public float extrusionHeight = 1.0f; // Adjust the extrusion height as needed

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

#if UNITY_EDITOR
    [ContextMenu("Update Mesh")]
    public void UpdateMeshButton()
    {
        // Remove existing mesh colliders
        RemoveMeshColliders();

        CinemachineSmoothPath spline = GetComponent<CinemachineSmoothPath>();
        if (spline != null)
        {
            TriangulateSpline(spline);
            // Add a new mesh collider
            AddMeshCollider();
        }
    }

    private void RemoveMeshColliders()
    {
        MeshCollider[] meshColliders = GetComponents<MeshCollider>();
        foreach (MeshCollider collider in meshColliders)
        {
            DestroyImmediate(collider);
        }
    }

    private void AddMeshCollider()
    {
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }
#endif

    private void TriangulateSpline(CinemachineSmoothPath spline)
    {
        if (meshFilter == null)
        {
            meshFilter = GetOrCreateMeshFilter();
        }

        if (spline == null)
        {
            UnityEngine.Debug.LogError("CinemachineSmoothPath component not found on the same GameObject!");
            return;
        }

        CinemachineSmoothPath.Waypoint[] waypoints = spline.m_Waypoints;
        List<Vector3> vertices = new List<Vector3>();
        List<Color32> colors = new List<Color32>();

        foreach (CinemachineSmoothPath.Waypoint waypoint in waypoints)
        {
            // Use the waypoint position as a vertex
            vertices.Add(waypoint.position);
            vertices.Add(new Vector3(waypoint.position.x, waypoint.position.y + extrusionHeight, waypoint.position.z));

            // Assign colors based on vertex position
            Color32 topColor = new Color32(255, 255, 255, 255); // White
            Color32 bottomColor = new Color32(128, 128, 128, 255); // Gray
            colors.Add(topColor);
            colors.Add(bottomColor);
        }

        // Connect first and last points
        vertices.Add(vertices[0]);
        vertices.Add(vertices[1]);
        colors.Add(new Color32(255, 255, 255, 255)); // White
        colors.Add(new Color32(128, 128, 128, 255)); // Gray

        // Triangulate the vertices
        int[] triangles = Triangulate(vertices);

        // Create a mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.colors32 = colors.ToArray(); // Assign vertex colors

        // Assign the mesh to the MeshFilter
        meshFilter.sharedMesh = mesh;

        // Optional: Recalculate normals and bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set a random name to the mesh
        meshFilter.sharedMesh.name = "SplineMesh_" + UnityEngine.Random.Range(1, 1000);
    }

    private int[] Triangulate(List<Vector3> vertices)
    {
        // Simple planar triangulation, assuming vertices are in order
        List<int> triangles = new List<int>();

        for (int i = 1; i < vertices.Count - 2; i += 2)
        {
            triangles.Add(i - 1);
            triangles.Add(i);
            triangles.Add(i + 1);

            triangles.Add(i + 1);
            triangles.Add(i);
            triangles.Add(i + 2);
        }

        // Add triangles for the top face
        for (int i = 0; i < vertices.Count - 2; i += 2)
        {
            triangles.Add(i);
            triangles.Add(i + 2);
            triangles.Add(vertices.Count - 2);
        }

        // Add triangles for the bottom face
        for (int i = 1; i < vertices.Count - 3; i += 2)
        {
            triangles.Add(i + 2);
            triangles.Add(i);
            triangles.Add(vertices.Count - 1);
        }

        return triangles.ToArray();
    }

    private MeshFilter GetOrCreateMeshFilter()
    {
        MeshFilter existingMeshFilter = GetComponent<MeshFilter>();
        if (existingMeshFilter != null)
        {
            return existingMeshFilter;
        }

        GameObject meshObject = new GameObject("SplineMesh");
        meshObject.transform.SetParent(transform);

        MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();

        // Optional: Assign a material to the MeshRenderer
        // meshRenderer.sharedMaterial = yourMaterial;

        return meshFilter;
    }
}
