using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Chunk;

public static class ChunkGeometry 
{
    static Vector3[] blockVerts =
    {
        new Vector3(0, 0, 0), //0
        new Vector3(1, 0, 0), //1
        new Vector3(1, 0, 1), //2
        new Vector3(0, 0, 1), //3
        new Vector3(0, 1, 0), //4
        new Vector3(1, 1, 0), //5
        new Vector3(1, 1, 1), //6
        new Vector3(0, 1, 1)
    }; //7

    private static Vector3[] BackVerts;
    private static Vector3[] RightVerts;
    private static Vector3[] FrontVerts;
    private static Vector3[] LeftVerts;
    private static Vector3[] BottomVerts;
    private static Vector3[] TopVerts;



    static int[] blockIndices = {
        //Back
        5, 1, 0,
        4, 5, 0,
        //Right
        6, 2, 1,
        5, 6, 1,
        //Front
        7, 3, 2,
        6, 7, 2,
        //Left
        4, 0, 3,
        7, 4, 3,
        //Bottom
        0, 1, 2,
        3, 0, 2,
        //Top
        6, 5, 4,
        7, 6, 4
       
    };

    private static Vector2[] UVs =
    {
        new Vector2(1/16f + ((1/16f) * 1), 1/16f + ((1/16f) * 15)),
        new Vector2(1/16f + ((1/16f) * 1), 0/16f+ ((1/16f) * 15)),
        new Vector2(0/16f + ((1/16f) * 1), 0/16f+ ((1/16f) * 15)),
        new Vector2(0/16f + ((1/16f) * 1), 1/16f+ ((1/16f) * 15)),
        new Vector2(1/16f + ((1/16f) * 1), 1/16f+ ((1/16f) * 15)),
        new Vector2(0/16f + ((1/16f) * 1), 0/16f+ ((1/16f) * 15))
    };
    


    private static Color curColor;

    static ChunkGeometry()
    {
        curColor = new Color(1,1,1);
        
        BackVerts = new Vector3[]
        {
            blockVerts[blockIndices[0]],
            blockVerts[blockIndices[1]],
            blockVerts[blockIndices[2]],
            blockVerts[blockIndices[3]],
            blockVerts[blockIndices[4]],
            blockVerts[blockIndices[5]]
        };
        
        RightVerts = new Vector3[]
        {
            blockVerts[blockIndices[6]],
            blockVerts[blockIndices[7]],
            blockVerts[blockIndices[8]],
            blockVerts[blockIndices[9]],
            blockVerts[blockIndices[10]],
            blockVerts[blockIndices[11]]
        };
    
        FrontVerts = new Vector3[]
        {
            blockVerts[blockIndices[12]],
            blockVerts[blockIndices[13]],
            blockVerts[blockIndices[14]],
            blockVerts[blockIndices[15]],
            blockVerts[blockIndices[16]],
            blockVerts[blockIndices[17]]
        };
        LeftVerts = new Vector3[]
        {
            blockVerts[blockIndices[18]],
            blockVerts[blockIndices[19]],
            blockVerts[blockIndices[20]],
            blockVerts[blockIndices[21]],
            blockVerts[blockIndices[22]],
            blockVerts[blockIndices[23]]
        };
        BottomVerts = new Vector3[]
        {
            blockVerts[blockIndices[24]],
            blockVerts[blockIndices[25]],
            blockVerts[blockIndices[26]],
            blockVerts[blockIndices[27]],
            blockVerts[blockIndices[28]],
            blockVerts[blockIndices[29]]
        };
        TopVerts = new Vector3[]
        {
            blockVerts[blockIndices[30]],
            blockVerts[blockIndices[31]],
            blockVerts[blockIndices[32]],
            blockVerts[blockIndices[33]],
            blockVerts[blockIndices[34]],
            blockVerts[blockIndices[35]]
        };
    }
    
    public static void ConstructBlock(int block, Chunk chunk, ref SubChunk subChunk, Faces faces, int x, int y, int z)
    {
        if (faces.back)
            constructBack(block, chunk, ref subChunk.Vertices, ref subChunk.Triangles, ref subChunk.Colors, ref subChunk.Uvs, faces, x, y, z);
        if(faces.right)
            constructRight(block, chunk, ref subChunk.Vertices, ref subChunk.Triangles, ref subChunk.Colors, ref subChunk.Uvs, faces, x,y,z);
        if(faces.forward)
            constructFront(block, chunk, ref subChunk.Vertices, ref subChunk.Triangles, ref subChunk.Colors, ref subChunk.Uvs, faces, x,y,z);
        if (faces.left)
            constructLeft(block, chunk, ref subChunk.Vertices, ref subChunk.Triangles, ref subChunk.Colors, ref subChunk.Uvs, faces, x, y, z);
        if(faces.up)
            constructTop(block, chunk, ref subChunk.Vertices, ref subChunk.Triangles, ref subChunk.Colors, ref subChunk.Uvs, faces, x,y,z);
        if(faces.down)
            constructBottom(block, chunk, ref subChunk.Vertices, ref subChunk.Triangles, ref subChunk.Colors, ref subChunk.Uvs, faces, x,y,z);
    }
    
    private static void constructBack(int block, Chunk chunk, ref List<Vector3> verts, ref List<int> inds, ref List<Color> cols, ref List<Vector2> uvs, Faces faces, int x, int y, int z) {

        LightLevel light = chunk.getLightLevel(x, y, z - 1);

        float r = light.r / 16f;
        float g = light.g / 16f;
        float b = light.b / 16f;

        curColor.r = r + 0.1f;
        curColor.g = g + 0.1f;
        curColor.b = b + 0.1f;

        foreach (var vert in BackVerts)
        {
            verts.Add(vert + new Vector3(x,y,z));
            inds.Add(verts.Count - 1);
            cols.Add(curColor * 0.8f);
        }
        
        uvs.AddRange(UVs);
        
    }
    private static void constructRight(int block, Chunk chunk, ref List<Vector3> verts, ref List<int> inds, ref List<Color> cols, ref List<Vector2> uvs, Faces faces, int x, int y, int z) {

        LightLevel light = chunk.getLightLevel(x + 1, y, z);
        float r = light.r / 16f;
        float g = light.g / 16f;
        float b = light.b / 16f;
        curColor.r = r + 0.1f;
        curColor.g = g + 0.1f;
        curColor.b = b + 0.1f;
        foreach (var vert in RightVerts)
        {
            verts.Add(vert + new Vector3(x,y,z));
            inds.Add(verts.Count - 1);
            cols.Add(curColor * 0.8f);
        }
        uvs.AddRange(UVs);

    }
    
    private static void constructFront(int block, Chunk chunk, ref List<Vector3> verts, ref List<int> inds, ref List<Color> cols, ref List<Vector2> uvs, Faces faces, int x, int y, int z) {
        LightLevel light = chunk.getLightLevel(x, y, z + 1);
        float r = light.r / 16f;
        float g = light.g / 16f;
        float b = light.b / 16f;

        curColor.r = r + 0.1f;
        curColor.g = g + 0.1f;
        curColor.b = b + 0.1f;
        foreach (var vert in FrontVerts)
        {
            verts.Add(vert + new Vector3(x,y,z));
            inds.Add(verts.Count - 1);
            cols.Add(curColor * 0.8f);
        }
        uvs.AddRange(UVs);

    }
    
    private static void constructLeft(int block, Chunk chunk, ref List<Vector3> verts, ref List<int> inds, ref List<Color> cols, ref List<Vector2> uvs, Faces faces, int x, int y, int z) {
        LightLevel light = chunk.getLightLevel(x - 1, y, z);
        float r = light.r / 16f;
        float g = light.g / 16f;
        float b = light.b / 16f;

        curColor.r = r + 0.1f;
        curColor.g = g + 0.1f;
        curColor.b = b + 0.1f;
        foreach (var vert in LeftVerts)
        {
            verts.Add(vert + new Vector3(x,y,z));
            inds.Add(verts.Count - 1);
            cols.Add(curColor * 0.8f);
        }
        uvs.AddRange(UVs);

    }
    
    private static void constructTop(int block, Chunk chunk, ref List<Vector3> verts, ref List<int> inds, ref List<Color> cols, ref List<Vector2> uvs, Faces faces, int x, int y, int z) {
        LightLevel light = chunk.getLightLevel(x, y + 1, z);
        float r = light.r / 16f;
        float g = light.g / 16f;
        float b = light.b / 16f;

        curColor.r = r + 0.1f;
        curColor.g = g + 0.1f;
        curColor.b = b + 0.1f;
        foreach (var vert in TopVerts)
        {
            verts.Add(vert + new Vector3(x,y,z));
            inds.Add(verts.Count - 1);
            cols.Add(curColor);
        }
        uvs.AddRange(UVs);

    }
    
    private static void constructBottom(int block, Chunk chunk, ref List<Vector3> verts, ref List<int> inds, ref List<Color> cols, ref List<Vector2> uvs, Faces faces, int x, int y, int z) {
        LightLevel light = chunk.getLightLevel(x, y - 1, z);
        float r = light.r / 16f;
        float g = light.g / 16f;
        float b = light.b / 16f;

        curColor.r = r + 0.1f;
        curColor.g = g + 0.1f;
        curColor.b = b + 0.1f;
        foreach (var vert in BottomVerts)
        {
            verts.Add(vert + new Vector3(x,y,z));
            inds.Add(verts.Count - 1);
            cols.Add(curColor * 0.4f);
        }
        uvs.AddRange(UVs);

    }
}
