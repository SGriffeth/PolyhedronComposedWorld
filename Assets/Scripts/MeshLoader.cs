using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshLoader : MonoBehaviour
{

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        MeshGenerator meshGenerator = new MeshGenerator(material);

        meshGenerator.GetGameObject(MeshGenerator.GetCube(new Vector3(0,0,0),MeshGenerator.GetCube(new Vector3(1,0,0))),"TempTest",true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
