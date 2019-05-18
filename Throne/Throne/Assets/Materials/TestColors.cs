using UnityEngine;

public class TestColors : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];

        colors[0] = Color.red;
        colors[1] = Color.red;
        colors[2] = Color.green;
        colors[3] = Color.red;

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
    }
}
