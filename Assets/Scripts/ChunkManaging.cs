namespace ChunkManaging
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class CubeComposedCircle {

        float x;
        float z;
        public float radius = 1;
        public float xSeparation = 1;
        public float zSeparation = 1;

        public float X{
            get {return x;}
        }

        public float Z{
            get {return z;}
        }

        public CubeComposedCircle(float x,float z){
            this.x = x;
            this.z = z;
        }

        public CubeComposedCircle(float x,float z,float radius) {
            this.x = x;
            this.z = z;
            this.radius = radius;
        }

        public bool ContainsPoint(float x,float z,bool bySeperation = false){
            if(bySeperation) return ((x % xSeparation == 0) && (z % zSeparation == 0)) && (Math.Pow(x-this.x,2) + Math.Pow(z-this.z,2) <= Math.Pow(radius,2));
            return Math.Pow(x-this.x,2) + Math.Pow(z-this.z,2) <= Math.Pow(radius,2);
        }

        public virtual Vector2[] GetPoints(){
            //Vector2[] coords = new Vector2[Convert.ToInt32(Math.Pow(radius*2+1,2))];
            //Debug.Log("Size : " + ChunkGenerator.RoundUp(Math.PI * Math.Pow(radius,2)));
            Vector2[] coords = new Vector2[ ChunkGenerator.RoundUp(Math.PI * Math.Pow(radius,2)) ];
            int i = 0;
            for(float z = this.z - radius;z <= this.z + radius;z++){
                for(float x = this.x - radius;x <= this.x + radius;x++){
                    if(ContainsPoint(x,z,true)) {
                        //Debug.Log("i " + i);
                        coords[i] = new Vector2(x * xSeparation,z * zSeparation);
                        i++;
                    }
                }
            }
            return coords;
        }

    }

    public class CubeComposedSphere : CubeComposedCircle
    {

        float y;
        public float ySeparation = 1;

        public float Y {
            get {return y;}
        }
        
        public CubeComposedSphere(float x,float y,float z) : base(x,z) {
            this.y = y;
        }

        public CubeComposedSphere(float x,float y,float z,float radius) : base(x,z,radius) {
            this.y = y;
        }

        public bool ContainsPoint(float x,float y,float z) {
            double a = Math.Pow(x-base.X,2) + Math.Pow(y-this.y,2); 
            double b = Math.Pow(z-base.Z,2);
            return a + b <= Math.Pow(base.radius,2);
        }

        public new Vector3[] GetPoints()
        {
            Vector3[] coords = new Vector3[Convert.ToInt32(Math.Pow(base.radius * 2 + 1, 3))];
            int i = 0;
            for (float y = this.y - base.radius; y <= this.y + base.radius; y++)
            {
                for (float z = base.Z - base.radius; z <= base.Z + base.radius; z++)
                {
                    for (float x = base.X - base.radius; x <= base.X + base.radius; x++)
                    {
                        if (ContainsPoint(x, y, z))
                        {
                            coords[i] = new Vector3(x * xSeparation, y * ySeparation, z * zSeparation);
                        }
                        i++;
                    }
                }
            }
            return coords;
        }

    }

    public static class ChunkGenerator {

        public static int GroundLayer = 3;
        public static string ChunkTag = "Chunk";
        public static List<Vector2> GeneratedPoints = new List<Vector2>();
        public static List<Vector2> GeneratedChunks = new List<Vector2>();
        public static Dictionary<Mesh,bool> Empty = new Dictionary<Mesh, bool>();
        public static List<Vector3[]> GeneratedVertices = new List<Vector3[]>();  

        public static Mesh GetChunk(Vector3 start,Vector3 end) {
            //GameObject gameObject = new GameObject("TempTest",typeof(MeshFilter),typeof(MeshRenderer));

            Mesh mesh = null;/*GetPolyhedron(new Vector3(0,0,0),new Vector3(0,0,1),new Vector3(1,0,0),
            new Vector3(0,-1,0),new Vector3(0,-1,1),new Vector3(1,-1,0),

            GetPolyhedron(new Vector3(1,0,0),new Vector3(0,0,1),new Vector3(1,0,1),
            new Vector3(1,-1,0),new Vector3(0,-1,1),new Vector3(1,-1,1))
            );*/

            for (float z = start.z,zPos = 0; z < end.z; z++)
            {

                for (float x = start.x,xPos = 0; x < end.x; x++)
                {
                    if(mesh == null) {
                        mesh = GetPolyhedron(new Vector3(xPos, Mathf.PerlinNoise(x * .3f,z * .3f) * 3, zPos),
                    new Vector3(xPos, Mathf.PerlinNoise(x * .3f,(z+1) * .3f) * 3, zPos + 1),
                    new Vector3(xPos + 1, Mathf.PerlinNoise((x+1) * .3f,z * .3f) * 3, zPos),
                    new Vector3(xPos, -1, zPos), new Vector3(xPos, -1, zPos + 1), new Vector3(xPos + 1, -1, zPos));
                    }else {
                        mesh = GetPolyhedron(new Vector3(xPos, Mathf.PerlinNoise(x * .3f,z * .3f) * 3, zPos),
                    new Vector3(xPos, Mathf.PerlinNoise(x * .3f,(z+1) * .3f) * 3, zPos + 1),
                    new Vector3(xPos + 1, Mathf.PerlinNoise((x+1) * .3f,z * .3f) * 3, zPos),
                    new Vector3(xPos, -1, zPos), new Vector3(xPos, -1, zPos + 1), new Vector3(xPos + 1, -1, zPos),

                    mesh);
                    }

                    mesh = GetPolyhedron(new Vector3(xPos+1,Mathf.PerlinNoise((x+1) * .3f,z * .3f) * 3,zPos),
                    new Vector3(xPos,Mathf.PerlinNoise(x * .3f,(z+1) * .3f) * 3,zPos+1),
                    new Vector3(xPos+1,Mathf.PerlinNoise((x+1) * .3f,(z+1) * .3f) * 3,zPos+1),
                    new Vector3(xPos+1,0-1,zPos),new Vector3(xPos,0-1,zPos+1),new Vector3(xPos+1,0-1,zPos+1),

                    mesh); 

                    xPos++;
                }
                zPos++;
            }

            return mesh;
/*
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshRenderer>().material = ChunkLoader.material;
*/
        }

        public static Mesh GetChunk(Vector2 start,Vector2 end,LayerMask mask,bool checkOverlap = false) {
            
            /*int xVertices = RoundUp(end.x) - RoundUp(start.x) + 1;
            int zVertices = RoundUp(end.y) - RoundUp(start.y) + 1;*/
            int xVertices = Convert.ToInt32(end.x - start.x + 1);
            int zVertices = Convert.ToInt32(end.y - start.y + 1);

            Vector3[] vertices = new Vector3[xVertices * zVertices];
            /*
            0       1       2       3
            (0,0,0) (1,0,0) (0,0,1) (1,0,1)
            */
            /*
            6
            (0,2,1) (1,2,3)
            */
            int[] triangles = new int[(xVertices-1) * (zVertices-1) * 6];
            
            int i = 0;
            int tri = 0;
            bool isEmpty = true;
            for(float z = start.y, zPos = 0;z <= end.y;z++) {
                for(float x = start.x, xPos = 0;x <= end.x;x++) {
                    FastNoiseLite noise = new FastNoiseLite();
                    noise.SetFrequency(0.01f);
                    noise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
                    float y = noise.GetNoise(x * 3f,z * 3f) * 10;
                    Debug.Log("y " + y);
                    vertices[i] = new Vector3(xPos, y, zPos);
                    if (GeneratedPoints.Contains(new Vector2(x, z)) && checkOverlap) {
                        //Debug.Log(x + "," + z);
                        xPos++;
                        i++;
                        continue;
                    }
                    if(xPos >= xVertices-1 || zPos >= zVertices-1) {xPos++;i++;continue;}

                    triangles[tri] = i;
                    triangles[tri + 1] = i + xVertices;
                    triangles[tri + 2] = i + 1;

                    triangles[tri + 3] = i + 1;
                    triangles[tri + 4] = i + xVertices;
                    triangles[tri + 5] = i + xVertices + 1;
                    isEmpty = false;
                    tri += 6;
                    i++;
                    xPos++;
                    if(checkOverlap)
                    GeneratedPoints.Add(new Vector2(x, z));
                }
                zPos++;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            Empty.Add(mesh,isEmpty);
            return mesh;
        }

        public static Mesh GetPolyhedron(Vector3 vertex,Vector3 vertex1,Vector3 vertex2,Vector3 vertex3,Vector3 vertex4,Vector3 vertex5) {
            Mesh result = new Mesh();

            Vector3[] vertices = new Vector3[6];

            vertices[0] = vertex;
            vertices[1] = vertex1;
            vertices[2] = vertex2;

            vertices[3] = vertex3;
            vertices[4] = vertex4;
            vertices[5] = vertex5;

            int[] triangles = new int[24];

            /*
            (0,0,0) (0,0,1) (1,0,0) | (0,-1,0) (0,-1,1) (1,-1,0)
            */

            if(!ChunkLoader.IsGenerated(vertices[0],vertices[1],vertices[2])) {
                triangles[0] = 0;
                triangles[1] = 1;
                triangles[2] = 2;
            }

            if(!ChunkLoader.IsGenerated(vertices[3],vertices[5],vertices[4])) {
                triangles[3] = 3;
                triangles[4] = 5;
                triangles[5] = 4;
            }

            if(!ChunkLoader.IsGenerated(vertices[3],vertices[0],vertices[5])) {
                triangles[6] = 3;
                triangles[7] = 0;
                triangles[8] = 5;
            }

            if(!ChunkLoader.IsGenerated(vertices[5],vertices[0],vertices[2])) {
                triangles[9] = 5;
                triangles[10] = 0;
                triangles[11] = 2;
            }

            if(!ChunkLoader.IsGenerated(vertices[3],vertices[4],vertices[0])) {
                triangles[12] = 3;
                triangles[13] = 4;
                triangles[14] = 0;   
            }

            if(!ChunkLoader.IsGenerated(vertices[4],vertices[1],vertices[0])) {
                triangles[15] = 4;
                triangles[16] = 1;
                triangles[17] = 0;   
            }

            if(!ChunkLoader.IsGenerated(vertices[5],vertices[2],vertices[4])) {
                triangles[18] = 5;
                triangles[19] = 2;
                triangles[20] = 4;   
            }

            if(!ChunkLoader.IsGenerated(vertices[4],vertices[2],vertices[1])) {
                triangles[21] = 4;
                triangles[22] = 2;
                triangles[23] = 1;   
            }

            for(int i = 0;i < triangles.Length;i+=3) {
                GeneratedVertices.Add(new Vector3[] {
                    vertices[triangles[i]],vertices[triangles[i+1]],vertices[triangles[i+2]],
                });
            }

            result.vertices = vertices;
            result.triangles = triangles;
            result.RecalculateNormals();
            return result;
        }

        public static Mesh GetPolyhedron(Vector3 vertex,Vector3 vertex1,Vector3 vertex2,Vector3 vertex3,Vector3 vertex4,Vector3 vertex5,Mesh polyhedron) {
            Vector3[] vector3s = new Vector3[polyhedron.vertexCount + 6]; // Check the size
            int[] triangles = new int[polyhedron.triangles.Length + 24];
            
            for(int i = 0;i < polyhedron.vertices.Length;i++) {
                vector3s[i] = polyhedron.vertices[i];
            }

            for(int i = 0;i < polyhedron.triangles.Length;i+=3) {
                triangles[i] = polyhedron.triangles[i];
                triangles[i+1] = polyhedron.triangles[i+1];
                triangles[i+2] = polyhedron.triangles[i+2];
            }

            vector3s[polyhedron.vertices.Length] = vertex;
            vector3s[polyhedron.vertices.Length+1] = vertex1;
            vector3s[polyhedron.vertices.Length+2] = vertex2;

            vector3s[polyhedron.vertices.Length+3] = vertex3;
            vector3s[polyhedron.vertices.Length+4] = vertex4;
            vector3s[polyhedron.vertices.Length+5] = vertex5;

            if(!ChunkLoader.IsGenerated(vector3s[0 + polyhedron.vertices.Length],
            vector3s[1 + polyhedron.vertices.Length],vector3s[2 + polyhedron.vertices.Length])) {
                triangles[0 + polyhedron.triangles.Length] = 0 + polyhedron.vertices.Length;
                triangles[1 + polyhedron.triangles.Length] = 1 + polyhedron.vertices.Length;
                triangles[2 + polyhedron.triangles.Length] = 2 + polyhedron.vertices.Length;
            }

            if(!ChunkLoader.IsGenerated(vector3s[3 + polyhedron.vertices.Length],
            vector3s[5 + polyhedron.vertices.Length],vector3s[4 + polyhedron.vertices.Length])) {
                triangles[3 + polyhedron.triangles.Length] = 3 + polyhedron.vertices.Length;
                triangles[4 + polyhedron.triangles.Length] = 5 + polyhedron.vertices.Length;
                triangles[5 + polyhedron.triangles.Length] = 4 + polyhedron.vertices.Length;   
            }

            if(!ChunkLoader.IsGenerated(vector3s[3 + polyhedron.vertices.Length],
            vector3s[0 + polyhedron.vertices.Length],vector3s[5 + polyhedron.vertices.Length])) {
                triangles[6 + polyhedron.triangles.Length] = 3 + polyhedron.vertices.Length;
                triangles[7 + polyhedron.triangles.Length] = 0 + polyhedron.vertices.Length;
                triangles[8 + polyhedron.triangles.Length] = 5 + polyhedron.vertices.Length;   
            }

            if(!ChunkLoader.IsGenerated(vector3s[5 + polyhedron.vertices.Length],
            vector3s[0 + polyhedron.vertices.Length],vector3s[2 + polyhedron.vertices.Length])) {
                triangles[9 + polyhedron.triangles.Length] = 5 + polyhedron.vertices.Length;
                triangles[10 + polyhedron.triangles.Length] = 0 + polyhedron.vertices.Length;
                triangles[11 + polyhedron.triangles.Length] = 2 + polyhedron.vertices.Length;   
            }

            if(!ChunkLoader.IsGenerated(vector3s[3 + polyhedron.vertices.Length],
            vector3s[4 + polyhedron.vertices.Length],vector3s[0 + polyhedron.vertices.Length])) {
                triangles[12 + polyhedron.triangles.Length] = 3 + polyhedron.vertices.Length;
                triangles[13 + polyhedron.triangles.Length] = 4 + polyhedron.vertices.Length;
                triangles[14 + polyhedron.triangles.Length] = 0 + polyhedron.vertices.Length;   
            }

            if(!ChunkLoader.IsGenerated(vector3s[4 + polyhedron.vertices.Length],
            vector3s[1 + polyhedron.vertices.Length],vector3s[0 + polyhedron.vertices.Length])) {
                triangles[15 + polyhedron.triangles.Length] = 4 + polyhedron.vertices.Length;
                triangles[16 + polyhedron.triangles.Length] = 1 + polyhedron.vertices.Length;
                triangles[17 + polyhedron.triangles.Length] = 0 + polyhedron.vertices.Length;   
            }
            
            if(!ChunkLoader.IsGenerated(vector3s[5 + polyhedron.vertices.Length],
            vector3s[2 + polyhedron.vertices.Length],vector3s[4 + polyhedron.vertices.Length])) {
                triangles[18 + polyhedron.triangles.Length] = 5 + polyhedron.vertices.Length;
                triangles[19 + polyhedron.triangles.Length] = 2 + polyhedron.vertices.Length;
                triangles[20 + polyhedron.triangles.Length] = 4 + polyhedron.vertices.Length;
            }

            if(!ChunkLoader.IsGenerated(vector3s[4 + polyhedron.vertices.Length],
            vector3s[2 + polyhedron.vertices.Length],vector3s[1 + polyhedron.vertices.Length])) {
                triangles[21 + polyhedron.triangles.Length] = 4 + polyhedron.vertices.Length;
                triangles[22 + polyhedron.triangles.Length] = 2 + polyhedron.vertices.Length;
                triangles[23 + polyhedron.triangles.Length] = 1 + polyhedron.vertices.Length;   
            }

            for(int i = polyhedron.triangles.Length;i <= polyhedron.triangles.Length + 23;i+=3) {
                GeneratedVertices.Add(new Vector3[] {
                    vector3s[triangles[i]],vector3s[triangles[i+1]],vector3s[triangles[i+2]],
                });
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vector3s;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Vector3[] GetVertices(Vector2 start,Vector2 end,float xNoise,float zNoise) {
            int xVertices = (RoundUp(end.x) - RoundUp(start.x));
            int zVertices = (RoundUp(end.y) - RoundUp(start.y));
            Vector3[] vertices = new Vector3[(xVertices + 1) * (zVertices + 1)];
            //Debug.Log(vertices.Length + " length, x " + end.x + " z " + end.y);
            int i = 0;
            for(float z = start.y;z <= end.y;z++) {
                for(float x = start.x;x <= end.x;x++) {
                    //Debug.Log("i : " + i + ", " + x + "," + z);
                    vertices[i] = new Vector3(x,Mathf.PerlinNoise(xNoise * .3f,zNoise * .3f)* 3f,z);
                    i++;
                    xNoise++;
                }
                zNoise++;    
            }
            return vertices;
        }

        public static int[] GetTriangles(Vector2 start,Vector2 end,Vector3[] vertices) {
            int xVertices = (RoundUp(end.x) - RoundUp(start.x));
            //int yVertices = (RoundUp(end.y) - RoundUp(start.y));
            int zVertices = (RoundUp(end.y) - RoundUp(start.y));
            int[] triangles = new int[xVertices * zVertices * 6];
            int i = 0;
            int tri = 0;
            for(float y = start.y;y <= end.y;y++) {
                for(float x = start.x;x <= end.x;x++) {
                    if(x >= xVertices || y >= zVertices) {
                        i++;
                        continue;
                    }
                    triangles[tri] = i;
                    triangles[tri+1] = i + xVertices + 1;
                    triangles[tri+2] = i + 1;

                    triangles[tri+3] = i + 1;
                    triangles[tri+4] = i + xVertices + 1;
                    triangles[tri+5] = i + xVertices + 2;

                    tri += 6;
                    i++;
                }
            }
            return triangles;
        }
        
        public static GameObject[] GetCubes(Vector3[] vertices,Vector2 start,Vector2 end,GameObject parent = null,string name = null) {
            int xVertices = (RoundUp(end.x) - RoundUp(start.x));
            int zVertices = (RoundUp(end.y) - RoundUp(start.y));
            GameObject[] cubes = new GameObject[xVertices * zVertices];
            int gameObject = 0;
            for (int vert = 0; vert < vertices.Length; vert++)
            {
                if (vertices[vert].x >= xVertices || vertices[vert].z >= zVertices)
                    continue;
                float x = vertices[vert].x;
                float z = vertices[vert].z;

                //Set the cube to be a ground layer
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if(name != null) cube.name = name + gameObject;
                cube.layer = GroundLayer;

                Vector3 scale = cube.transform.localScale;
                float average = (vertices[vert].y + vertices[vert + 1].y + vertices[vert + xVertices + 1].y + vertices[vert + xVertices + 2].y) / 4f;
                cube.transform.localPosition = new Vector3(x + (scale.x / 2f), average + (scale.y / 2f), z + (scale.z / 2f));

                cubes[gameObject] = cube;
                if(parent != null) cube.transform.parent = parent.transform;
                gameObject++;
            }
            return cubes;
        }

        public static Mesh GetMesh(Vector2 start,Vector2 end,float xNoise,float zNoise) {
            Vector3[] vertices = ChunkGenerator.GetVertices(start,end,xNoise,zNoise);
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = ChunkGenerator.GetTriangles(start,end,vertices);
            mesh.RecalculateNormals();
            return mesh;
        }

        public static int RoundDown(float x) {
            //x = 1.5
            int result = (int) x;
            if(result > x) result--;
            return result;
        }

        public static int RoundUp(float x) {
            int result = (int) x;
            if(result < x) result++;
            return result;
        }

        public static int RoundUp(double x) {
            int result = (int) x;
            if(result < x) result++;
            return result;
        }
    }

    public class Vector
    {
        
        float _x;
        float _y;
        float _z;

        public float X  {
            get {return _x;}
        }

        public float Y {
            get {return _y;}
        }

        public float Z {
            get {return _z;}
        }

        public Vector(float x,float y,float z) {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public static string ToString(Vector vector) {
            return "(" + vector._x + ", " + vector._y + ", " + vector._z + ")";
        }

    }

}