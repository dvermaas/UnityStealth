using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Mesh_Generator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    Vector2[] uvs;
    int[] triangles;

    public int xSize = 80;
    public int zSize = 295;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        // Create all the points
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Make middle flat
                if (39 < x && x < 41)
                {
                    float y = 0;
                    vertices[i] = new Vector3(x, y, z);
                }
                else
                {
                    float alt_z;
                    if (z >= zSize)
                    {
                        alt_z = 0;
                    }
                    else
                    {
                        alt_z = z;
                    }
                    if (x <= 36)
                    {
                        float y = Mathf.PerlinNoise(x * .1f, alt_z * .1f) * ((8 * (39 - x)) / 39) * 1.5f;
                        vertices[i] = new Vector3(x, y, z);
                    }
                    else
                    {
                        float y = Mathf.PerlinNoise(x * .1f, alt_z * .1f) * ((8 * (x-39))/39) * 1.5f;
                        vertices[i] = new Vector3(x, y, z);
                    }
                }
                i++;
            }
        }
        int uvversion = 1;
        if (uvversion == 1)
        {
            // Add uv's data
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            }
        }
        else
        {
            // uv2
            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                    i++;
                }
            }
        }
        

        // Fill mesh with triangles
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }
}
