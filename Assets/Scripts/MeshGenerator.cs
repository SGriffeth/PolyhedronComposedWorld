using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{

    public static int GroundLayer = 3;

    public Material material;

    public MeshGenerator(Material material) {
        this.material = material;
    }

    public GameObject GetGameObject(Mesh mesh,string name,bool collider = true) {
        GameObject gameObject = new GameObject(name,typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider));

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        if(collider)
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        gameObject.layer = GroundLayer;
        return gameObject;
    }

    public static Mesh GetTriangle(Vector3 vertex,Vector3 vertex1,Vector3 vertex2) {
        float distanceA = Vector3.Distance(vertex,vertex1);
        float distanceC = Vector3.Distance(vertex1,vertex2);
        float distanceB = Vector3.Distance(vertex,vertex2);
        
        if( (distanceA == 1 && distanceB == 1 && distanceC == Math.Sqrt(2)) || 
        (distanceA == 1 && distanceC == 1 && distanceB == Math.Sqrt(2)) ||
        (distanceB == 1 && distanceC == 1 && distanceA == Math.Sqrt(2))) {
            Debug.Log("sides are 1,1 and sqrt(2)");
        }

        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            vertex,vertex1,vertex2
        };
        mesh.triangles = new int[] {
            0,1,2
        };
        mesh.RecalculateNormals();
        return mesh;
    }

    public static Mesh GetTriangle(Vector3 vertex,Vector3 vertex1,Vector3 vertex2,Mesh mesh) {
        return null;
    }

    public static Mesh GetCube(Vector3 position) {
        // Generate top square
        Mesh mesh = MeshGenerator.GetTriangle(new Vector3(position.x,position.y+1,position.z),
        new Vector3(position.x,position.y+1,position.z+1),new Vector3(position.x+1,position.y+1,position.z),

        MeshGenerator.GetTriangle(new Vector3(position.x+1,position.y+1,position.z),
        new Vector3(position.x,position.y+1,position.z+1),new Vector3(position.x+1,position.y+1,position.z+1)));

        // Generate right square
        mesh = MeshGenerator.GetTriangle(new Vector3(position.x+1,position.y,position.z),
        new Vector3(position.x+1,position.y+1,position.z),new Vector3(position.x+1,position.y+1,position.z+1),mesh);

        mesh = MeshGenerator.GetTriangle(new Vector3(position.x+1,position.y,position.z),
        new Vector3(position.x+1,position.y+1,position.z+1),new Vector3(position.x+1,position.y,position.z+1),mesh);

        // Generate front square
        mesh = MeshGenerator.GetTriangle(new Vector3(position.x+1,position.y,position.z+1),
        new Vector3(position.x+1,position.y+1,position.z+1),new Vector3(position.x,position.y,position.z+1),mesh);

        mesh = MeshGenerator.GetTriangle(new Vector3(position.x,position.y,position.z+1),
        new Vector3(position.x+1,position.y+1,position.z+1),new Vector3(position.x,position.y+1,position.z+1),mesh);

        // Generate left square
        mesh = MeshGenerator.GetTriangle(position,new Vector3(position.x,position.y,position.z+1),
        new Vector3(position.x,position.y+1,position.z),mesh);

        mesh = MeshGenerator.GetTriangle(new Vector3(position.x,position.y,position.z+1),
        new Vector3(position.x,position.y+1,position.z+1),new Vector3(position.x,position.y+1,position.z),mesh);
        
        // Generate back square
        mesh = MeshGenerator.GetTriangle(position,new Vector3(position.x,position.y+1,position.z),
        new Vector3(position.x+1,position.y,position.z),mesh);

        mesh = MeshGenerator.GetTriangle(new Vector3(position.x+1,position.y,position.z),
        new Vector3(position.x,position.y+1,position.z),new Vector3(position.x+1,position.y+1,position.z),mesh);

        // Generate bottom square
        mesh = MeshGenerator.GetTriangle(new Vector3(position.x,position.y,position.z+1),
        position,new Vector3(position.x+1,position.y,position.z+1),mesh);

        mesh = MeshGenerator.GetTriangle(new Vector3(position.x+1,position.y,position.z+1),
        position,new Vector3(position.x+1,position.y,position.z),mesh);

        return mesh;
    }

    public static Mesh GetCube(Vector3 position,Mesh mesh) {
        Vector3[] vertices = new Vector3[mesh.vertices.Length+8];
        int[] triangles = new int[mesh.triangles.Length + 36];

        for(int i = 0;i < mesh.vertices.Length;i++) {
            vertices[i] = mesh.vertices[i];
        }

        for(int i = 0;i < mesh.triangles.Length;i+=3) {
            triangles[i] = mesh.triangles[i];
            triangles[i+1] = mesh.triangles[i+1];
            triangles[i+2] = mesh.triangles[i+2];
        }

        vertices[mesh.vertices.Length] = new Vector3(0,0,0);
        vertices[mesh.vertices.Length+1] = new Vector3(1,0,0);
        vertices[mesh.vertices.Length+2] = new Vector3(0,0,1);
        vertices[mesh.vertices.Length+3] = new Vector3(1,0,1);

        vertices[mesh.vertices.Length+4] = new Vector3(0,1,0);
        vertices[mesh.vertices.Length+5] = new Vector3(1,1,0);
        vertices[mesh.vertices.Length+6] = new Vector3(0,1,1);
        vertices[mesh.vertices.Length+7] = new Vector3(1,1,1);

        // Generate top square
        triangles[mesh.triangles.Length] = mesh.vertices.Length+4;
        triangles[mesh.triangles.Length+1] = mesh.vertices.Length+6;
        triangles[mesh.triangles.Length+2] = mesh.vertices.Length+5;

        triangles[mesh.triangles.Length+3] = mesh.vertices.Length+5;
        triangles[mesh.triangles.Length+4] = mesh.vertices.Length+6;
        triangles[mesh.triangles.Length+5] = mesh.vertices.Length+7;

        // Generate right square
        triangles[mesh.triangles.Length+6] = mesh.vertices.Length+5;
        triangles[mesh.triangles.Length+7] = mesh.vertices.Length+7;
        triangles[mesh.triangles.Length+8] = mesh.vertices.Length+1;

        triangles[mesh.triangles.Length+9] = mesh.vertices.Length+1;
        triangles[mesh.triangles.Length+10] = mesh.vertices.Length+7;
        triangles[mesh.triangles.Length+11] = mesh.vertices.Length+3;

        // Generate front square
        triangles[mesh.triangles.Length+12] = mesh.vertices.Length+6;
        triangles[mesh.triangles.Length+13] = mesh.vertices.Length+2;
        triangles[mesh.triangles.Length+14] = mesh.vertices.Length+7;

        triangles[mesh.triangles.Length+15] = mesh.vertices.Length+7;
        triangles[mesh.triangles.Length+16] = mesh.vertices.Length+2;
        triangles[mesh.triangles.Length+17] = mesh.vertices.Length+3;

        // Generate left square
        triangles[mesh.triangles.Length+18] = mesh.vertices.Length+0;
        triangles[mesh.triangles.Length+19] = mesh.vertices.Length+2;
        triangles[mesh.triangles.Length+20] = mesh.vertices.Length+4;

        triangles[mesh.triangles.Length+21] = mesh.vertices.Length+4;
        triangles[mesh.triangles.Length+22] = mesh.vertices.Length+2;
        triangles[mesh.triangles.Length+23] = mesh.vertices.Length+6;

        // Generate back square
        triangles[mesh.triangles.Length+24] = mesh.vertices.Length+0;
        triangles[mesh.triangles.Length+25] = mesh.vertices.Length+4;
        triangles[mesh.triangles.Length+26] = mesh.vertices.Length+1;

        triangles[mesh.triangles.Length+27] = mesh.vertices.Length+1;
        triangles[mesh.triangles.Length+28] = mesh.vertices.Length+4;
        triangles[mesh.triangles.Length+29] = mesh.vertices.Length+5;

        // Generate bottom square
        triangles[mesh.triangles.Length+30] = mesh.vertices.Length+2;
        triangles[mesh.triangles.Length+31] = mesh.vertices.Length+0;
        triangles[mesh.triangles.Length+32] = mesh.vertices.Length+3;

        triangles[mesh.triangles.Length+33] = mesh.vertices.Length+3;
        triangles[mesh.triangles.Length+34] = mesh.vertices.Length+0;
        triangles[mesh.triangles.Length+35] = mesh.vertices.Length+1;

        Mesh mesh1 = new Mesh();
        mesh1.vertices = vertices;
        mesh1.triangles = triangles;
        mesh1.RecalculateNormals();
        return mesh1;
    }

    /*
    public static Mesh GetTriangle(Vector3 vertex,Vector3 vertex1,Vector3 vertex2,Mesh mesh) {
        Vector3[] vertices = new Vector3[mesh.vertices.Length + 3];
        int[] triangles = new int[mesh.triangles.Length + 3];

        for(int i = 0;i < mesh.vertices.Length;i++) {
            vertices[i] = mesh.vertices[i];
        }

        for(int i = 0;i < mesh.triangles.Length;i+=3) {
            triangles[i] = mesh.triangles[i];
            triangles[i + 1] = mesh.triangles[i + 1];
            triangles[i + 2] = mesh.triangles[i + 2];
        }

        vertices[mesh.vertices.Length] = vertex;
        vertices[mesh.vertices.Length+1] = vertex1;
        vertices[mesh.vertices.Length+2] = vertex2;

        triangles[mesh.triangles.Length] = mesh.triangles.Length;
        triangles[mesh.triangles.Length+1] = mesh.triangles.Length+1;
        triangles[mesh.triangles.Length+2] = mesh.triangles.Length+2;

        Mesh mesh1 = new Mesh();
        mesh1.vertices = vertices;
        mesh1.triangles = triangles;
        mesh1.RecalculateNormals();
        
        return mesh1;
    }
    */

    /*public static void Simplify(Mesh mesh) {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for(int i = 0;i < mesh.triangles.Length;i+=3) {
            if(Array.Exists(mesh.triangles,indice => vertices[indice] == vertices[triangles[i]]) &&) {
                int indice1 = Array.Find(mesh.triangles,indice => vertices[indice] == vertices[triangles[i]]);
            }
        }

    }*/

    /*public static Mesh Combine(Mesh mesh, Mesh mesh1) {
        Vector3[] vertices = new Vector3[mesh.vertices.Length + mesh1.vertices.Length];
        int[] triangles = new int[mesh.triangles.Length + mesh1.triangles.Length];

        for(int i = 0;i < mesh.vertices.Length;i++) {
            vertices[i] = mesh.vertices[i];
        }

        for(int i = 0;i < mesh.triangles.Length;i+=3) {
            triangles[i] = mesh.triangles[i];
            triangles[i+1] = mesh.triangles[i+1];
            triangles[i+2] = mesh.triangles[i+2];
        }

        for(int i = 0;i < mesh1.vertices.Length;i++) {
            vertices[i+mesh.vertices.Length] = mesh1.vertices[i];
        }

        for(int i = 0;i < mesh1.triangles.Length;i+=3) {
            triangles[i+0+mesh.triangles.Length] = mesh1.triangles[i+0];
            triangles[i+1+mesh.triangles.Length] = mesh1.triangles[i+1];
            triangles[i+2+mesh.triangles.Length] = mesh1.triangles[i+2];
        }

        Mesh mesh2 = new Mesh();
        mesh2.vertices = vertices;
        mesh2.triangles = triangles;
        mesh2.RecalculateNormals();
        return mesh2;
    }*/

    /*public static Mesh Simplify(Mesh mesh) {
        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        int[] triangles = new int[mesh.triangles.Length];
        Debug.Log("vertices " + vertices.Length);
        Debug.Log("triangles " + triangles.Length);

        for(int i = 0;i < vertices.Length;i++) {
            vertices[i] = mesh.vertices[i];
        }

        for(int i = 0;i < triangles.Length;i+=3) {
            // Check if there exist 3 indices which refer to the same 3 vertices as the indices in the current iteration
            if(Array.Exists(triangles,indice => vertices[indice] == vertices[triangles[i]]) &&
            Array.Exists(triangles,indice => vertices[indice] == vertices[triangles[i+1]]) &&
            Array.Exists(triangles,indice => vertices[indice] == vertices[triangles[i+2]])) {
                Debug.Log("i " + i + "");
                // If true then Find the three indices and remove the corresponding triangle and do not load the current iteration
                int indice = Array.Find(triangles,indice => vertices[indice] == vertices[triangles[i]]);
                int indice1 = Array.Find(triangles,indice => vertices[indice] == vertices[triangles[i+1]]);
                int indice2 = Array.Find(triangles,indice => vertices[indice] == vertices[triangles[i+2]]);

                vertices[indice] = new Vector3(0,0,0);
                vertices[indice1] = new Vector3(0,0,0);
                vertices[indice2] = new Vector3(0,0,0);
            }else {
                // Or else simply load the triangle
                triangles[i] = i;
                triangles[i + 1] = i + 1;
                triangles[i + 2] = i + 2;
            }
        }

        Mesh mesh1 = new Mesh();
        mesh1.vertices = vertices;
        mesh1.triangles = triangles;
        mesh1.RecalculateNormals();
        return mesh1;
    }*/

    /*public static bool HasBeenGenerated(Vector3 vertex,Vector3 vertex1,Vector3 vertex2,List<Vector3[]> GeneratedVertices) {
        return GeneratedVertices.Exists(x => Array.Exists(x,vertice => vertice == vertex) &&
        Array.Exists(x,vertice => vertice == vertex1) && Array.Exists(x,vertice => vertice == vertex2)
        );        
    }*/
    
    /*public void DisplayGeneratedVertices(string name) {
        for(int i = 0;i < GeneratedVertices.Count;i++) {
            Mesh mesh = new Mesh();

            Vector3[] vertices = GeneratedVertices[i];
            int[] triangles = new int[] {
                0,1,2
            };
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            GetGameObject(mesh,name + i);
        }
    }*/

    /*public static Mesh Combine(Mesh mesh,Mesh mesh1) {
        Vector3[] vertices = new Vector3[mesh.vertices.Length + mesh1.vertices.Length];
        int[] triangles = new int[mesh.triangles.Length + mesh1.triangles.Length];
        Debug.Log("mesh " + mesh.vertices.Length + ", mesh1 " + mesh1.vertices.Length + " vertices " + vertices.Length + ", triangles " + triangles.Length);

        for(int i = 0;i < mesh.vertices.Length;i++) {
            Debug.Log("i1 " + i);
            vertices[i] = mesh.vertices[i];
        }

        for(int i = mesh.vertices.Length;i < vertices.Length;i++) {
            Debug.Log("i2 " + i + " mesh1(i) " + (i - mesh.vertices.Length));
            vertices[i] = mesh1.vertices[i - mesh.vertices.Length];
        }

        for(int i = 0;i < mesh.triangles.Length;i+=3) {
            triangles[i] = mesh.triangles[i];
            triangles[i+1] = mesh.triangles[i+1];
            triangles[i+2] = mesh.triangles[i+2];
        }

        for(int i = mesh.triangles.Length;i < triangles.Length;i+=3) {
            triangles[i] = mesh1.triangles[i - mesh.triangles.Length];
            triangles[i+1] = mesh1.triangles[i+1 - mesh.triangles.Length];
            triangles[i+2] = mesh1.triangles[i+2 - mesh.triangles.Length];
        }

        Mesh mesh2 = new Mesh();
        mesh2.vertices = vertices;
        mesh2.triangles = triangles;
        mesh2.RecalculateNormals();
        return mesh2;
    }*/

}
