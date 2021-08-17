using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ChunkManaging;

public class ChunkLoader : MonoBehaviour
{

    public Material material;
    public int chunkLength = 1;
    public float renderDistance = 3;
    public Transform chunkLoader;
    public LayerMask mask;
    public static List<Vector2> generatedChunks = new List<Vector2>();
    public Vector3 vertex;
    public Vector3 vertex1;
    public Vector3 vertex2;
    public Vector3 vertex3;
    public Vector3 vertex4;
    public Vector3 vertex5;
    public Vector3 start;
    public Vector3 end;

    // Start is called before the first frame update
    void Start(){
        /*Mesh mesh = ChunkGenerator.GetPolyhedron(new Vector3(vertex.x+1,vertex.y,vertex.z),new Vector3(vertex1.x+1,vertex1.y,vertex1.z),
        new Vector3(vertex2.x+1,vertex2.y,vertex2.z),new Vector3(vertex3.x+1,vertex3.y,vertex3.z),
        new Vector3(vertex4.x+1,vertex4.y,vertex4.z),new Vector3(vertex5.x+1,vertex5.y,vertex5.z),

        ChunkGenerator.GetPolyhedron(vertex,vertex1,vertex2,vertex3,vertex4,vertex5));*/

        Mesh mesh = ChunkGenerator.GetPolyhedron(vertex,vertex1,vertex2,vertex3,vertex4,vertex5,
        
        ChunkGenerator.GetPolyhedron(new Vector3(1,0,0),new Vector3(0,0,1),new Vector3(1,0,1),
        new Vector3(1,-1,0),new Vector3(0,-1,1),new Vector3(1,-1,1)));

        mesh = ChunkGenerator.GetPolyhedron(new Vector3(vertex.x+1,vertex.y,vertex.z),new Vector3(vertex1.x+1,vertex1.y,vertex1.z),
        new Vector3(vertex2.x+1,vertex2.y,vertex2.z),new Vector3(vertex3.x+1,vertex3.y,vertex3.z),
        new Vector3(vertex4.x+1,vertex4.y,vertex4.z),new Vector3(vertex5.x+1,vertex5.y,vertex5.z),
        mesh);

        GetGameObject(mesh,"TemTesterrr");
        //ShowGeneratedTriangles("TempTest");
        /*GameObject gameObject1 = new GameObject("TempTester",typeof(MeshFilter),typeof(MeshRenderer));

        Mesh mesh = ChunkGenerator.GetPolyhedron(vertex,vertex1,vertex2,vertex3,vertex4,vertex5,

        ChunkGenerator.GetPolyhedron(new Vector3(1,0,0),new Vector3(0,0,1),new Vector3(1,0,1),
        new Vector3(1,-1,0),new Vector3(0,-1,1),new Vector3(1,-1,1)));

        gameObject1.GetComponent<MeshFilter>().mesh = mesh;
        gameObject1.GetComponent<MeshRenderer>().material = material;

        foreach(Vector[] vertices in ChunkGenerator.GeneratedTriangles) {
            foreach(Vector vertex in vertices) {
                Debug.Log(vertex.X + "," + vertex.Y + "," + vertex.Z);
            }
            Debug.Log(ChunkGenerator.GeneratedTriangles.Contains(new Vector[] {
                vertices[0],vertices[1],vertices[2]
            }) + " " + ChunkManaging.Vector.ToString(vertices[0]) + ", " + ChunkManaging.Vector.ToString(vertices[1])
            + ", " + ChunkManaging.Vector.ToString(vertices[2]));

            GameObject gameObject = new GameObject("TempTest",typeof(MeshFilter),typeof(MeshRenderer));
            Mesh mesh1 = new Mesh();
            mesh1.vertices = new Vector3[] {
                new Vector3(vertices[0].X,vertices[0].Y,vertices[0].Z),
                new Vector3(vertices[1].X,vertices[1].Y,vertices[1].Z),
                new Vector3(vertices[2].X,vertices[2].Y,vertices[2].Z),
            };

            mesh1.triangles = new int[3] {
                0,1,2
            };
            mesh1.RecalculateNormals();
            gameObject.GetComponent<MeshFilter>().mesh = mesh1;
            gameObject.GetComponent<MeshRenderer>().material = material;
        }*/
    }

    private void Update()
    {
        //int x = (int)chunkLoader.transform.position.x;
        //int z = (int)chunkLoader.transform.position.z;
        //if(!ChunkGenerator.VisistedPlaces.Contains(new Vector2(x,z))) {
        /*CubeComposedCircle cubeComposed = new CubeComposedCircle(x, z, radius);
        CreateChunk(cubeComposed);*/
        //CreateChunkSquare(new Vector2(x-chunkLength/2,z-chunkLength/2),new Vector2(x+chunkLength/2,z+chunkLength/2));
        //LoadChunks();
            /*
            GameObject gameObject = new GameObject(x + "," + z,typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider));
            Mesh mesh = ChunkGenerator.GetCircle(cubeComposed);
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshRenderer>().material = material;
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            gameObject.layer = ChunkGenerator.GroundLayer;
            gameObject.transform.position = new Vector3(x,0,z);
            ChunkGenerator.VisistedPlaces.Add(new Vector2(x,z));
            */
            
            /*ChunkGenerator.VisistedPlaces.Add(new Vector2(x,z));
        }*/
        //LoadChunks();
        //LoadChunks3();
    }
    
    private void OnDrawGizmosSelected() {
        foreach(Vector2 generated in generatedChunks) {
            Gizmos.DrawSphere(new Vector3(generated.x,0,generated.y),.2f);
        }
    }

    void ShowGeneratedTriangles(string name) {
        foreach(Vector3[] vertices in ChunkGenerator.GeneratedVertices) {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = new int[] {
                0,1,2
            };
            mesh.RecalculateNormals();

            GameObject gameObject = new GameObject(name,typeof(MeshFilter),typeof(MeshRenderer));
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshRenderer>().material = material;

            Debug.Log(IsGenerated(vertices[0],vertices[2],vertices[1]));
            /*Debug.Log(ChunkGenerator.GeneratedVertices.Contains(new Vector3[] {
                vertices[0],vertices[1],vertices[2],
            }) + "," +
            ChunkGenerator.GeneratedVertices.Contains(new Vector3[] {
                vertices[0],vertices[2],vertices[1],
            }) + "," +
            ChunkGenerator.GeneratedVertices.Contains(new Vector3[] {
                vertices[1],vertices[0],vertices[2],
            }) + "," +
            ChunkGenerator.GeneratedVertices.Contains(new Vector3[] {
                vertices[1],vertices[2],vertices[0],
            }) + "," +
            ChunkGenerator.GeneratedVertices.Contains(new Vector3[] {
                vertices[2],vertices[0],vertices[1],
            }) + "," +
            ChunkGenerator.GeneratedVertices.Contains(new Vector3[] {
                vertices[2],vertices[1],vertices[0],
            }));*/
        }
    }

    public static bool IsGenerated(Vector3 vertex,Vector3 vertex1,Vector3 vertex2) {
        //return false;
        return ChunkGenerator.GeneratedVertices.Exists(x => Array.Exists(x, vertice => vertice == vertex) &&
         Array.Exists(x, vertice => vertice == vertex1) &&
         Array.Exists(x, vertice => vertice == vertex2));
    }

    public GameObject GetGameObject(Mesh mesh,string name) {
        GameObject gameObject = new GameObject(name,typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider));
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.layer = ChunkGenerator.GroundLayer;
        return gameObject;
    }

    void LoadChunks() {
        int chunkX = NearestMultiple(chunkLoader.position.x,chunkLength);
        int chunkZ = NearestMultiple(chunkLoader.position.z,chunkLength);
        for (int z = chunkZ - (int)(renderDistance/2 * chunkLength); z < chunkZ + (int)(renderDistance/2 * chunkLength); z += chunkLength)
        {
            for (int x = chunkX - (int)(renderDistance/2 * chunkLength); x < chunkX + (int)(renderDistance/2 * chunkLength); x += chunkLength)
            {
                if(generatedChunks.Contains(new Vector2(x,z))) continue;
                CreateChunkSquare(new Vector2(x,z),new Vector2(x+chunkLength,z+chunkLength));
                generatedChunks.Add(new Vector2(x,z));
            }
        }
    }

    void LoadChunks3() {
        int chunkX = NearestMultiple(chunkLoader.position.x,chunkLength);
        int chunkZ = NearestMultiple(chunkLoader.position.z,chunkLength);

        for(int z = chunkZ - (int)renderDistance/2 * chunkLength;z < chunkZ + (int)renderDistance/2 * chunkLength;z += chunkLength) {
            for(int x = chunkX - (int)renderDistance/2 * chunkLength;x < chunkX + (int)renderDistance/2 * chunkLength;x += chunkLength) {
                if(generatedChunks.Contains(new Vector2(x,z))) continue;
                Mesh mesh = ChunkGenerator.GetChunk(new Vector3(x, 0, z), new Vector3(x + chunkLength, 0, z + chunkLength));

                GameObject chunk = new GameObject("TempTest", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

                chunk.GetComponent<MeshFilter>().mesh = mesh;
                chunk.GetComponent<MeshRenderer>().material = material;
                chunk.GetComponent<MeshCollider>().sharedMesh = mesh;
                chunk.layer = ChunkGenerator.GroundLayer;
                chunk.transform.position = new Vector3(x,0,z);

                generatedChunks.Add(new Vector2(x,z));
            }
        }
    }

    GameObject CreateChunkSquare(Vector2 start, Vector2 end)
    {
        if (ChunkGenerator.GeneratedChunks.Contains(new Vector2(start.x, start.y))) return null;

        Mesh mesh = ChunkManaging.ChunkGenerator.GetChunk(start, end, mask);
        if (ChunkGenerator.Empty[mesh])
        {
            //Debug.Log("EMPTY " + start.x + "," + start.y);
            return null;
        }
        GameObject gameObject = new GameObject(start.x + "," + start.y, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        // HHEUH FUIHE
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        gameObject.layer = ChunkGenerator.GroundLayer;
        gameObject.transform.position = new Vector3(start.x, 0, start.y);
        gameObject.tag = ChunkGenerator.ChunkTag;

        ChunkGenerator.GeneratedChunks.Add(new Vector2(start.x, start.y));

        return gameObject;
    }

    GameObject CreatePolyhedron(Vector3 vertex,Vector3 vertex1,Vector3 vertex2,Vector3 vertex3,Vector3 vertex4,Vector3 vertex5) {
        GameObject gameObject = new GameObject("(" + vertex.x + "," + vertex.y + ","
         + vertex.z + ") (" + vertex.x + "," + vertex.y + "," + vertex.z + ")",typeof(MeshFilter),typeof(MeshRenderer));
        Mesh mesh = ChunkGenerator.GetPolyhedron(vertex,vertex1,vertex2,vertex3,vertex4,vertex5,ChunkGenerator.GetPolyhedron(vertex,vertex1,vertex2,vertex3,vertex4,vertex5));
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        return gameObject;
    }

    public static int NearestMultiple(int x, int multiple)
    {
        int rem = x % multiple;
        if (rem > multiple / 2) return x + multiple - rem;
        return x - rem;
    }

    public static int NearestMultiple(double x, int multiple)
    {
        int aprox = Convert.ToInt32(x);
        int rem = aprox % multiple;
        if (rem > multiple / 2) return aprox + multiple - rem;
        return aprox - rem;
    }

    public static int NearestMultiple(float x, int multiple)
    {
        // 3
        int aprox = Convert.ToInt32(x);
        // 3 % 2
        int rem = aprox % multiple;
        // 1 >= 2/2
        if (rem > multiple / 2) return aprox + multiple - rem; // 3 + 2 - 1
        return aprox - rem;
    }

    /*GameObject SpawnChunk2(GameObject parent,string childName,Material material,Vector2 start,Vector2 end,float xNoise,float zNoise,bool cubical = false) {
        Mesh mesh = ChunkGenerator.GetMesh(start,end,xNoise,zNoise);

        if(cubical) {
            ChunkGenerator.GetCubes(mesh.vertices,start,end,parent,childName);
            return parent;
        }

        Chunk2 chunk = new Chunk2(start,end);
        GameObject child = new GameObject(childName,typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider));
        child.layer = ChunkGenerator.GroundLayer;
        child.GetComponent<MeshFilter>().mesh = mesh;
        child.GetComponent<MeshRenderer>().material = material;
        child.GetComponent<MeshCollider>().sharedMesh = mesh;
        child.transform.parent = parent.transform;
        return parent;
    }

    GameObject CreateChunk(CubeComposedCircle circle) {
        float x  = circle.X;
        float z  = circle.Z;
        if(ChunkGenerator.GeneratedChunks.Contains(new Vector2(x,z))) return null;
        Mesh mesh = ChunkGenerator.GetCircle(circle,mask);
        GameObject gameObject = new GameObject(x + "," + z, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        //gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.layer = ChunkGenerator.GroundLayer;
        gameObject.transform.position = new Vector3(x, 0, z);
        gameObject.tag = ChunkGenerator.ChunkTag;
        //ChunkGenerator.ChunkArea.Add(gameObject,circle);
        ChunkGenerator.GeneratedChunks.Add(new Vector2(x,z));
        return gameObject;
    }*/

}
