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

        /*public float Radius{
            get {return radius;}
            set {radius = value;}
        }*/

        public CubeComposedCircle(float x,float z){
            this.x = x;
            this.z = z;
        }

        public CubeComposedCircle(float x,float z,float radius) {
            this.x = x;
            this.z = z;
            this.radius = radius;
        }

        void GetSize() {
            
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
                    //i++; //NO?
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

        public static Mesh GetCircle(CubeComposedCircle circle,LayerMask mask) {
            Mesh mesh = new Mesh();
            Vector2 start = new Vector2(circle.X - circle.radius,circle.Z - circle.radius);
            Vector2 end = new Vector2(circle.X + circle.radius,circle.Z + circle.radius);

            int xVertices = RoundUp(end.x) - RoundUp(start.x) + 1;
            int zVertices = RoundUp(end.y) - RoundUp(start.y) + 1;

            Debug.Log(xVertices + " width");
            Debug.Log(zVertices + " depth");

            //bool doesOverlap;
            Vector3[] vertices = new Vector3[ xVertices * zVertices ];
            //Debug.Log("length " + vertices.Length);
            int[] triangles = new int[(xVertices-1) * (zVertices-1) * 6];
            int i = 0;
            int tri = 0;
            for (float z = start.y, zPos = -circle.radius; z <= end.y; z++)
            {
                for (float x = start.x, xPos = -circle.radius; x <= end.x; x++)
                {

                    if (GeneratedPoints.Contains(new Vector2(x, z))) {
                        //Debug.Log(x + "," + z);
                        xPos++;
                        i++;
                        continue;
                    }

                    float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 3;
                    vertices[i] = new Vector3(xPos, y, zPos);

                    int newIndices = 0;
                    if (circle.ContainsPoint(x, z, true) && circle.ContainsPoint(x + 1, z, true) &&
                    circle.ContainsPoint(x, z + 1, true))
                    {
                        triangles[tri] = i;
                        triangles[tri + 1] = i + xVertices;
                        triangles[tri + 2] = i + 1;
                        newIndices += 3;
                    }

                    if (circle.ContainsPoint(x + 1, z, true) &&
                    circle.ContainsPoint(x, z + 1, true) && circle.ContainsPoint(x + 1, z + 1, true))
                    {
                        triangles[tri + 3] = i + 1;
                        triangles[tri + 4] = i + xVertices;
                        triangles[tri + 5] = i + xVertices + 1;

                        newIndices += 3;
                    }

                    tri += newIndices;
                    i++;
                    xPos++;

                    if(xPos < xVertices-1 && zPos < zVertices-1) {
                        GeneratedPoints.Add(new Vector2(x, z));
                        Debug.Log(x + "," + z);
                    }
                }
                zPos++;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            return mesh;
        }

        public static Mesh GetSquare(Vector2 start,Vector2 end,LayerMask mask,bool checkOverlap = false) {
            int xVertices = RoundUp(end.x) - RoundUp(start.x) + 1;
            int zVertices = RoundUp(end.y) - RoundUp(start.y) + 1;

            Vector3[] vertices = new Vector3[xVertices * zVertices];
            int[] triangles = new int[(xVertices-1) * (zVertices-1) * 6];
            
            int i = 0;
            int tri = 0;
            bool isEmpty = true;
            for(float z = start.y, zPos = 0;z <= end.y;z++) {
                for(float x = start.x, xPos = 0;x <= end.x;x++) {
                    float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 3;
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

        public static Mesh GetTetrahedron(Vector3 vertex,Vector3 vertex1,Vector3 vertex2,Vector3 vertex3) {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4];
            int[] triangles = new int[12];
            // (0,0) (0,1) (1,0) (0,-1,0)
            vertices[0] = vertex;
            vertices[1] = vertex1;
            vertices[2] = vertex2;

            vertices[3] = vertex3;

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            triangles[3] = 3;
            triangles[4] = 1;
            triangles[5] = 0;

            mesh.vertices = vertices;
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

       /* public static int[] GetTriangles(CubeComposedCircle circle,Vector3[] vertices) {
            float radius = circle.radius;
            Vector2 start = new Vector2(circle.X - radius,circle.Z - radius);
            Vector2 end = new Vector2(circle.X + radius,circle.Z + radius);
            int xVertices = RoundUp(end.x) - RoundUp(start.x);
            Debug.Log("X Vertices " + xVertices);

            int[] triangles = new int[ ChunkGenerator.RoundUp(Math.PI * Math.Pow(radius,2)) * 6];
            Debug.Log("triangles " + triangles.Length);
            int i = 0;
            int tri = 0;
            for(float z = start.y;z <= end.y;z++) {
                for(float x = start.x;x <= end.x;x++) {
                    if(!circle.ContainsPoint(x,z) || !circle.ContainsPoint(x,z+1) || !circle.ContainsPoint(x+1,z) || !circle.ContainsPoint(x+1,z+1)) {
                        //Debug.Log(x + "," + z);
                        //Debug.Log("con i " + i);
                        i++;
                        continue;
                    }
                    Debug.Log("i " + i);
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
        }*/

        /*public static void GetTriangles(Vector2 start,Vector2 end,Vector3[] vertices) {
            bool centerCounted = false;

            float xVertices = RoundUp(end.x) - RoundUp(start.x);
            float zVertices = RoundUp(end.y) - RoundUp(start.y);

            for(int i = 0;i < vertices.Length;i++) {
                Vector3 vertex = vertices[i];
                if(centerCounted && (vertex.x == 0 && vertex.y == 0 && vertex.z == 0)) {continue;}
                if(vertex.x)
                if(vertex.x == 0 && vertex.y == 0 && vertex.z == 0) {
                    centerCounted = true;
                }
            }


        }*/

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

    public class Chunk2 {

        Vector2 start;
        Vector2 end;
        //public GameObject _Chunk;

        public Vector2 Start {
            get {return start;}
        }

        public Vector2 End {
            get {return end;}
        }

        public Chunk2(Vector2 start,Vector2 end) {
            this.start = start;
            this.end = end;
        }

        /*public Chunk2(Vector2 start,Vector2 end,GameObject chunk) {
            this.start = start;
            this.end = end;
            this._Chunk = chunk;
        }*/

        public Mesh GetMesh(float xNoise,float zNoise) {
            Vector3[] vertices = ChunkGenerator.GetVertices(start,end,xNoise,zNoise);
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = ChunkGenerator.GetTriangles(start,end,vertices);
            mesh.RecalculateNormals();
            return mesh;
        }

    }

    public class Chunk {

        GameObject chunk;
        //public GameObject[] GameObjects;
        public int XVertices = 16;
        public int ZVertices = 16;
        public static int GroundLayer = 3;

        public GameObject _Chunk {
            get {return chunk;}
        }

        public Chunk(Vector3 position) {
            chunk = new GameObject("Chunk(" + position.x + "," + position.y + "," + position.z + ")" + "," + XVertices + "," + ZVertices);
            chunk.transform.position = position;
        }

        public Chunk(Vector3 position,int width,int depth) {
            chunk = new GameObject("Chunk(" + position.x + "," + position.y + "," + position.z + ")" + "," + width + "," + depth);
            chunk.transform.position = position;
            this.XVertices = width;
            this.ZVertices = depth;
        }

        public Mesh GetMesh(float noiseX,float noiseZ) {
            Mesh mesh = new Mesh();
            //GameObject gameObject = new GameObject(name,typeof(MeshFilter),typeof(MeshRenderer),typeof(MeshCollider));
            //gameObject.transform.parent = chunk.transform;
            Vector3[] vertices = new Vector3[(XVertices + 1) * (ZVertices + 1)];
            int i = 0;
            for (float z1 = 0; z1 <= ZVertices; z1++)
            {
                for (float x1 = 0; x1 <= XVertices; x1++)
                {
                    float y1 = Mathf.PerlinNoise(noiseX * .3f, noiseZ * .3f) * 3f;
                    vertices[i] = new Vector3(x1, y1, z1);
                    i++;
                    noiseX++;
                }
                noiseZ++;
            }
            mesh.vertices = vertices;
            mesh.triangles = GetTriangles(vertices,chunk.transform.position, new Vector2(chunk.transform.position.x + XVertices, chunk.transform.position.z + ZVertices));
            mesh.RecalculateNormals();
            return mesh;
            /*gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            gameObject.layer = GroundLayer;*/
            /*
            Old way of setting triangles
            int[] triangles = new int[XVertices * ZVertices * 6];
            int triIndex = 0;
            for (int vertex = 0; vertex <= vertices.Length && triIndex < triangles.Length; vertex++)
            {
                if (vertices[vertex].x >= XVertices/* + x*/ // || vertices[vertex].z >= ZVertices/* + z*/)
                /*{
                    continue;
                }
                triangles[triIndex + 0] = vertex;
                triangles[triIndex + 1] = vertex + XVertices + 1;
                triangles[triIndex + 2] = vertex + 1;

                triangles[triIndex + 3] = vertex + 1;
                triangles[triIndex + 4] = vertex + XVertices + 1;
                triangles[triIndex + 5] = vertex + XVertices + 2;

                triIndex += 6;
            }*/
        }

        public void CreateCubes(Vector3[] vertices) {
            int gameObject = 0;
            for (int vert = 0; vert < vertices.Length; vert++)
            {
                if (vertices[vert].x >= XVertices || vertices[vert].z >= ZVertices)
                    continue;
                float x = vertices[vert].x;
                float z = vertices[vert].z;

                //Set the cube to be a ground layer
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.layer = GroundLayer;
                cube.transform.parent = chunk.transform;

                Vector3 scale = cube.transform.localScale;
                float average = (vertices[vert].y + vertices[vert + 1].y + vertices[vert + XVertices + 1].y + vertices[vert + XVertices + 2].y) / 4f;
                cube.transform.localPosition = new Vector3(x + (scale.x / 2f), average, z + (scale.z / 2f));
                gameObject++;
            }
        }

        public static int[] GetTriangles(Vector3[] vertices,Vector2 start,Vector2 end) {
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

        static int RoundDown(float x) {
            int result = (int) x;
            if(result > x) result--;
            return result;
        }

        static int RoundUp(float x) {
            int result = (int) x;
            if(result < x) result++;
            return result;
        }

        /*public static void Cut(GameObject chunk,string name,Vector3 start,Vector3 end) {
            Mesh mesh = chunk.GetComponent<MeshFilter>().mesh;
            int xVertices = (RoundUp(end.x) - RoundUp(start.x));
            int yVertices = (RoundUp(end.y) - RoundUp(start.y));
            int zVertices = (RoundUp(end.z) - RoundUp(start.z));
            Vector3[] vertices = new Vector3[ xVertices * yVertices * zVertices ];
            Vector3[] vertices1 = chunk.GetComponent<MeshFilter>().mesh.vertices;
            for(int i = 0;i < vertices.Length;i++) {
                vertices[i] = vertices1[i];
            }
                        
        }*/

    }

}