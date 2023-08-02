using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ReverseObject : MonoBehaviour
{
    public int screening = 0;
    public bool changeCollider = true;
    public bool removeExistingColliders = true;

    private void Start()
    {
        InvertMesh();
        if (changeCollider)
            CreateInvertedMeshCollider();

        gameObject.transform.GetComponent<MeshFilter>().mesh.triangles.Reverse().ToArray();
    }

    public void CreateInvertedMeshCollider()
    {
        if (removeExistingColliders)
            RemoveExistingColliders();

        gameObject.AddComponent<MeshCollider>();
    }

    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}
