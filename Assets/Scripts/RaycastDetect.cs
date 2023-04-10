using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RaycastDetect : MonoBehaviour
{
    public float range = 100f;

    [HideInInspector]
    public Camera fpsCam;
    public Transform cube;


    [SerializeField] Material mat;

    public KeyCode keyCode;

    public string materialname;


    // Start is called before the first frame update
    void Start()
    {
        fpsCam = GetComponent<Camera>();

        MaterialGet();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 50;
        Debug.DrawLine(transform.position, forward, Color.green, range);

        if(Input.GetKeyDown(keyCode))
        {
            MaterialGet();
        }

        cubeRotate();
    }
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 500, 20), "Material Name : "+materialname);

        GUI.Label(new Rect(10, 30, 500, 20), "Rotate Cube : W , A , S , D");
        GUI.Label(new Rect(10, 50, 500, 20), "Get Material Name : Space");
        if (GUI.Button(new Rect(10, 70, 200, 30), "ResetCube"))
        {
            cube.rotation=Quaternion.Euler(0, 0, 0);    
        }
    }
    void MaterialGet()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            MeshCollider meshCollider = hit.collider as MeshCollider;

            Mesh mesh = meshCollider.sharedMesh;
            Renderer renderer = hit.collider.GetComponent<MeshRenderer>();

            Debug.Log(mesh.subMeshCount);
            int[] hitTriangle = new int[]
                    {
                            mesh.triangles[hit.triangleIndex * 3],
                            mesh.triangles[hit.triangleIndex * 3 + 1],
                            mesh.triangles[hit.triangleIndex * 3 + 2]
                    };

            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                int[] subMeshTris = mesh.GetTriangles(i);
                for (int j = 0; j < subMeshTris.Length; j += 3)
                {
                    if (subMeshTris[j] == hitTriangle[0] &&
                        subMeshTris[j + 1] == hitTriangle[1] &&
                        subMeshTris[j + 2] == hitTriangle[2])
                    {
                        mat = renderer.materials[i];
                        materialname=mat.name;
                    }
                }
            }
        }
    }
    void cubeRotate()
    {
        cube.transform.Rotate(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"), 0);
    }
}
