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
    public Material meshMaterial; // Material for the MeshRenderer

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

#if UNITY_EDITOR
    [ContextMenu("Update Mesh")]
    public void UpdateMeshButton()
    {
        // Remove existing MeshCollider
        RemoveComponent<MeshCollider>();

        // Check if MeshFilter and MeshRenderer exist, if not, add them
        if (GetComponent<MeshFilter>() == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        else
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        if (GetComponent<MeshRenderer>() == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        else
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        // Assign the material to the MeshRenderer
        if (meshMaterial != null)
        {
            meshRenderer.sharedMaterial = meshMaterial;
        }

        // Triangulate and add new MeshCollider
        CinemachineSmoothPath spline = GetComponent<CinemachineSmoothPath>();
        if (spline != null)
        {
            TriangulateSpline(spline);
            AddMeshCollider();
        }
    }

    private void RemoveComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component != null)
        {
            DestroyImmediate(component);
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

        foreach (CinemachineSmoothPath.Waypoint waypoint in waypoints)
        {
            // Use the waypoint position as a vertex
            vertices.Add(waypoint.position);
            vertices.Add(new Vector3(waypoint.position.x, waypoint.position.y + extrusionHeight, waypoint.position.z));
        }

        // Connect first and last points
        vertices.Add(vertices[0]);
        vertices.Add(vertices[1]);

        // Triangulate the vertices
        int[] triangles = Triangulate(vertices);

        // Create a mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

        // Set vertex colors
        SetVertexColors(mesh);

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

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        // Optional: Assign a material to the MeshRenderer
        if (meshMaterial != null)
        {
            meshRenderer.sharedMaterial = meshMaterial;
        }

        return meshFilter;
    }

    private void SetVertexColors(Mesh mesh)
    {
        Color32[] colors = new Color32[mesh.vertices.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            // Assign white color to the top vertices, and gray to the bottom vertices
            colors[i] = i % 2 == 0 ? Color.white : Color.gray;
        }

        mesh.colors32 = colors;
    }
}
