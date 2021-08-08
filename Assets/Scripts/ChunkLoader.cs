using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ChunkManaging;

public class ChunkLoader : MonoBehaviour
{

    public Material material;
    /*public static Vector2 start;
    public static Vector2 end;
    public float xNoise = start.x;
    public float zNoise = start.y;*/
    public int chunkLength = 1;
    public float renderDistance = 3;
    public Transform chunkLoader;
    public LayerMask mask;
    public static List<Vector2> generatedChunks = new List<Vector2>();
    /*public Material material;
    private Vector3[][] gizmos = new Vector3[ (ChunkGenerator.RoundUp(end.x) - ChunkGenerator.RoundUp(start.x)) * (ChunkGenerator.RoundUp(end.x) - ChunkGenerator.RoundUp(start.x)) ][];*/

    // Start is called before the first frame update
    void Start()
    {
        //LoadChunks();
    }

    private void Update()
    {
        LoadChunks();
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
    
    private void OnDrawGizmosSelected() {
        /*foreach(Vector2 generated in ChunkGenerator.GeneratedPoints) {
            Gizmos.DrawSphere(new Vector3(generated.x,0,generated.y),.2f);;
        }*/
        foreach(Vector2 generated in generatedChunks) {
            Gizmos.DrawSphere(new Vector3(generated.x,0,generated.y),.2f);
        }
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

}
