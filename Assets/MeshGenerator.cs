using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class MeshGenerator : MonoBehaviour
{
    public Material material;
    private Mesh cubeMesh;
    private Matrix4x4 matrix;
    public float width = 1f;
    public float height = 1f;
    public float depth = 1f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 90f;

    void Start()
    {
        // Create the cube mesh
        CreateCubeMesh();
        
        // Initialize the matrix with identity (position at origin, no rotation, scale 1)
        matrix = Matrix4x4.identity;
    }

    void CreateCubeMesh()
    {
        cubeMesh = new Mesh();
        
        // Cube vertices (8 vertices)
        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(0, 0, 0),        
            new Vector3(width, 0, 0),     
            new Vector3(0, height, 0),   
            new Vector3(width, height, 0),
            new Vector3(0, 0, depth),     
            new Vector3(width, 0, depth), 
            new Vector3(0, height, depth),
            new Vector3(width, height, depth)
        };
        
        int[] tris = new int[36]
        {
            // Front face
            0, 2, 1,
            2, 3, 1,
            // Back face
            5, 7, 4,
            7, 6, 4,
            // Left face
            4, 6, 0,
            6, 2, 0,
            // Right face
            1, 3, 5,
            3, 7, 5,
            // Top face
            2, 6, 3,
            6, 7, 3,
            // Bottom face
            0, 1, 4,
            1, 5, 4
        };
        
        Vector3[] normals = new Vector3[8]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.back,
            -Vector3.back,
            -Vector3.back,
            -Vector3.back
        };
        
        Vector2[] uv = new Vector2[8]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        cubeMesh.vertices = vertices;
        cubeMesh.triangles = tris;
        cubeMesh.normals = normals;
        cubeMesh.uv = uv;
        cubeMesh.RecalculateNormals();
    }

    void Update()
    {
        // movement (A/D)
        float moveInput = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        
        // rotation (W/S)
        float rotateInput = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;
        
        Vector3 position = matrix.GetColumn(3);
        Quaternion rotation = matrix.rotation;
        Vector3 scale = matrix.lossyScale;
        
        position.x += moveInput * moveSpeed * Time.deltaTime;
        
        rotation *= Quaternion.Euler(0, rotateInput * rotationSpeed * Time.deltaTime, 0);
        
        matrix = Matrix4x4.TRS(position, rotation, scale);
        
        Graphics.DrawMesh(cubeMesh, matrix, material, 0);
    }

}

