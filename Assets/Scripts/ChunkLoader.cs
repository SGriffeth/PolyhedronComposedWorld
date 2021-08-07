using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChunkManaging;

public class ChunkLoader : MonoBehaviour
{

    public Material material;
    /*public static Vector2 start;
    public static Vector2 end;
    public float xNoise = start.x;
    public float zNoise = start.y;*/
    public float radius = 3;
    public Transform chunkLoader;
    public LayerMask mask;
    /*public Material material;
    private Vector3[][] gizmos = new Vector3[ (ChunkGenerator.RoundUp(end.x) - ChunkGenerator.RoundUp(start.x)) * (ChunkGenerator.RoundUp(end.x) - ChunkGenerator.RoundUp(start.x)) ][];*/

    // Start is called before the first frame update
    void Start()
    {
        GameObject tetrahedron = new GameObject("Tetrahedron",typeof(MeshFilter),typeof(MeshRenderer));
        tetrahedron.GetComponent<MeshFilter>().mesh = ChunkGenerator.GetTetrahedron(new Vector3(0,0,0),new Vector3(0,0,1),new Vector3(1,0,0),new Vector3(0,-1,0));
        tetrahedron.GetComponent<MeshRenderer>().material = material;
        /*CubeComposedCircle cubeComposed = new CubeComposedCircle(20,20,radius);
        
        Mesh mesh =
        ChunkGenerator.GetCircle(cubeComposed);
        GameObject gameObject = new GameObject("irk",typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider));
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.layer = ChunkGenerator.GroundLayer;*/
        /*Vector2 start = new Vector2(0,0);
        Vector2 end = new Vector2(6,6);*/
        /*Vector3[] vertices = ChunkGenerator.GetVertices(start,end,xNoise,zNoise);
        int i = 0;
        for (float z = start.y; z <= end.y; z++)
        {
            for (float x = start.x; x <= end.x; x++)
            {
                //Debug.Log("i : " + i + ", " + x + "," + z);
                vertices[i] = new Vector3(x, Mathf.PerlinNoise(xNoise * .3f, zNoise * .3f) * 3f, z);
                i++;
                xNoise++;
            }
            zNoise++;
        }*/
        /*
        string name = "part_"/*"p(X" + start.x + "Z" + start.y + ")(X" + end.x + "Z" + end.y + ")"*/;
        /*SpawnChunk2(new GameObject("CubicalChunk"),name,material,start,end,xNoise,zNoise,true);
        */
        /*CubeComposedCircle cubeComposed = new CubeComposedCircle(start.x,start.y,radius);
        cubeComposed.xSeparation = 5;
        cubeComposed.zSeparation = 5;
        Vector2[] points = cubeComposed.GetPoints();
        */
    }

    private void Update()
    {
        int x = (int)chunkLoader.transform.position.x;
        int z = (int)chunkLoader.transform.position.z;
        //if(!ChunkGenerator.VisistedPlaces.Contains(new Vector2(x,z))) {
        /*CubeComposedCircle cubeComposed = new CubeComposedCircle(x, z, radius);
        CreateChunk(cubeComposed);*/
        CreateChunkSquare(new Vector2(x-radius/2,z-radius/2),new Vector2(x+radius/2,z+radius/2));
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
    }

    private void OnDrawGizmosSelected() {
        foreach(Vector2 generated in ChunkGenerator.GeneratedPoints) {
            Gizmos.DrawSphere(new Vector3(generated.x,0,generated.y),.2f);;
        }
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
    }

    GameObject CreateChunkSquare(Vector2 start,Vector2 end) {
        if(ChunkGenerator.GeneratedChunks.Contains(new Vector2(start.x,start.y))) return null;

        Mesh mesh = ChunkManaging.ChunkGenerator.GetSquare(start,end,mask);
        if(ChunkGenerator.Empty[mesh]) {
            //Debug.Log("EMPTY " + start.x + "," + start.y);
            return null;
        }
        GameObject gameObject = new GameObject(start.x + "," + start.y, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        gameObject.layer = ChunkGenerator.GroundLayer;
        gameObject.transform.position = new Vector3(start.x,0,start.y);
        gameObject.tag = ChunkGenerator.ChunkTag;

        ChunkGenerator.GeneratedChunks.Add(new Vector2(start.x,start.y));

        return gameObject;
    }

    //private void Update() {
        
        /*mesh.vertices = vertices;
        mesh.triangles = ChunkGenerator.GetTriangles(new Vector2(cubeComposed.X - radius,cubeComposed.Z - radius),new Vector2(cubeComposed.X + radius,cubeComposed.Z + radius),vertices);
        mesh.RecalculateNormals();
        GameObject gameObject = new GameObject("",typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider));
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;*/
    //}

    GameObject SpawnChunk2(GameObject parent,string childName,Material material,Vector2 start,Vector2 end,float xNoise,float zNoise,bool cubical = false) {
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

    /*private void OnDrawGizmosSelected() {
        CubeComposedCircle cubeComposed = new CubeComposedCircle(start.x,start.y,radius);
        Vector2[] points = cubeComposed.GetPoints();
        for(int i = 0;i < points.Length;i++) {
            Vector3 point = new Vector3(points[i].x,0,points[i].y);
            Debug.Log(i + " " + point.x + "," + point.y);
            Gizmos.DrawSphere(point,.2f);
        }
    }*/

    /*private void OnDrawGizmosSelected() {
        CubeComposedCircle cubeComposed = new CubeComposedCircle(chunkLoader.position.x,chunkLoader.position.z,radius);
        Vector3[] points = ChunkGenerator.GetMesher(cubeComposed);
        for(int i = 0;i < points.Length;i++) {
            Vector3 point = new Vector3(points[i].x,0,points[i].z);
            Debug.Log(i + " " + point.x + "," + point.y);
            Gizmos.DrawSphere(point,.2f);
        }
    }*/

    /*private void OnDrawGizmosSelected() {
        foreach(Vector3 overlap in ChunkGenerator.overlaps) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(overlap,.3f);
        }
    }*/

}
