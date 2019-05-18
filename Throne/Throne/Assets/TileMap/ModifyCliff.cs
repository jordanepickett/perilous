using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ModifyCliff : MonoBehaviour
{
    protected Vector3[] verts;
    protected Mesh mesh;
    protected MeshCollider meshCollider;
    protected MeshFilter oMeshFilter;
    protected Mesh oMesh;
    protected Mesh cMesh;
    protected Vector3[] vertices;
    protected int[] triangles;
    bool isCloned = false;

    private Transform handleTransform;
    private Quaternion handleRotation;

    // Use this for initialization
    void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        meshCollider = GetComponent<MeshCollider>();
        //verts = new Vector3[mesh.vertices.Length];
        ////for (int i = 0; i < mesh.vertices.Length; i++)
        ////{
        ////    verts[i] = transform.TransformPoint(mesh.vertices[i]);
        ////    print(verts[i]);
        ////}
        ////mesh.vertices = verts;
        ////mesh.RecalculateNormals();
        ////mesh.RecalculateBounds();
        ////meshCollider.sharedMesh = mesh;
        //CloneMesh();
    }

    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        //for (var i = 0; i < vertices.Length; i++)
        //{
        //    vertices[i].z += .001f;
        //}
        //vertices[5].z += .001f;

        mesh.vertices = vertices;
    }

    public void ModifyVerteces(Vector3 t, bool lowerHeight)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        t.x = Mathf.RoundToInt(t.x / 1);
        t.z = Mathf.RoundToInt(t.z / 1);
        //print(t);

        //vertices[5].z += .001f;

        //Vector3 w = transform.TransformPoint(vertices[3]);
        //float distance = Vector3.Distance(w, t);
        //print(distance);
        float radius = .3f;

        for (int index = 0; index < vertices.Length; index++)
        {
            Vector3 w = transform.TransformPoint(vertices[index]);
            Vector3 roundedCliff = new Vector3(w.x, 0, w.z);
            Vector3 roundedPoint = new Vector3(t.x, 0, t.z);
            float distance = Vector3.Distance(roundedCliff, roundedPoint);
            var test = transform.InverseTransformPoint(t);
            //print(verts[index]);
            //if vert is within the radius 
            if (distance < radius)
            {
                //print(test);
                vertices[index] = test;
                //print(t);
                //mesh.vertices = vertices;
                //if (lowerHeight)
                //{
                //    //newVert.z -= 0.02f;
                //    vertices[index].z -= 0.0002f;
                //}
                //else
                //{
                //    vertices[index].z += 0.0002f;
                //}
            }
        }

        mesh.vertices = vertices;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        //meshCollider.sharedMesh = mesh;
    }

    void CloneMesh()
    {
        oMeshFilter = GetComponent<MeshFilter>();
        oMesh = oMeshFilter.sharedMesh; //1

        cMesh = new Mesh(); //2
        cMesh.name = "clone";
        cMesh.vertices = oMesh.vertices;
        cMesh.triangles = oMesh.triangles;
        cMesh.normals = oMesh.normals;
        cMesh.uv = oMesh.uv;
        oMeshFilter.mesh = cMesh;  //3

        vertices = cMesh.vertices; //4
        triangles = cMesh.triangles;
        isCloned = true;
        Debug.Log("Init & Cloned");
        //EditMesh();

        handleTransform = transform; //1
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity; //2
        for (int i = 0; i < cMesh.vertices.Length; i++) //3
        {
            ShowPoint(i);
        }
    }

    void ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(cMesh.vertices[index]);
        //Handles.color = Color.blue;
        // print(handleRotation);
        //point = Handles.FreeMoveHandle(point, handleRotation, 0.03f, Vector3.zero, Handles.DotHandleCap);
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(point, .3f);
    }

    // To test Reset function
    public void EditMesh()
    {
        vertices[2] = new Vector3(vertices[2].x, vertices[2].y + 0.2f, vertices[2].z);
        print(vertices[2]);
        vertices[3] = transform.TransformPoint(new Vector3(vertices[3].x, vertices[2].y + 0.2f, vertices[3].z));
        cMesh.vertices = vertices;
        cMesh.RecalculateNormals();
    }

}
