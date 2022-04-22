using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubChunk : MonoBehaviour
{
    //[HideInInspector]
    public List<Vector3> Vertices;
    [HideInInspector]
    public List<int> Triangles;
    [HideInInspector]
    public List<Color> Colors;
    [HideInInspector]
    public List<Vector2> Uvs;

    public int[,,] data;

    public byte[,,] lightInfoSky;

    public byte[,,] lightInfoR;
    public byte[,,] lightInfoG;
    public byte[,,] lightInfoB;

    public const int CHUNK_WIDTH = 16, CHUNK_LENGTH = 16, CHUNK_HEIGHT = 16;

    void Awake()
    {
        data = new int[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_LENGTH];

        lightInfoSky = new byte[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_LENGTH];

        lightInfoR = new byte[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_LENGTH];
        lightInfoG = new byte[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_LENGTH];
        lightInfoB = new byte[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_LENGTH];
        Vertices = new List<Vector3>();
        Triangles = new List<int>();
        Colors = new List<Color>();
        Uvs = new List<Vector2>();
    }

    public void SetMesh()
    {
        Mesh m = GetMesh();
        m.Clear();

        m.vertices = Vertices.ToArray();
        m.triangles = Triangles.ToArray();


        Vector3[] colorChannel = new Vector3[Colors.Count];

        for (int i = 0; i < Colors.Count; i++)
        {
            colorChannel[i] = new Vector3(Colors[i].r, Colors[i].g, Colors[i].b);
        }


        m.uv = Uvs.ToArray();
        m.SetUVs(1, colorChannel);

        //m.RecalculateBounds();
        m.RecalculateNormals();
        m.RecalculateBounds();
        m.RecalculateTangents();
    }

    public Mesh GetMesh()
    {

        MeshFilter mf = this.GetComponent<MeshFilter>();
        if (mf == null)
        {
            mf = this.gameObject.AddComponent<MeshFilter>();
            mf.mesh = new Mesh();
        }
        return mf.mesh;
    }
}
