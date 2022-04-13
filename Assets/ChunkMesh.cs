using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Schema;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Sequence = DG.Tweening.Sequence;


public class ChunkMesh : MonoBehaviour
{

    public class Faces
    {
        public bool up, down, left, right, forward, back;

        public Faces(bool up, bool down, bool left, bool right, bool forward, bool back)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.forward = forward;
            this.back = back;
        }

        public Faces()
        {
            this.up = false;
            this.down = false;
            this.left = false;
            this.right = false;
            this.forward = false;
            this.back = false;
        }
    }

    


    public const int CHUNK_WIDTH = 16, CHUNK_LENGTH = 16, CHUNK_HEIGHT = 16;
    private const int cwidth = 16, clength = 16, cheight = 16;
    private const int chunkStackHeight = CHUNK_HEIGHT / cheight; // 16 * 8 = 128;

    public Vector2 position;
    
    int[,,] data = new int[16,16,16];
    
    
    
    public Material material = null;

    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Color> colors;
    private List<Vector2> uvs;
    
    
    
    void Start()
    {
        
        position = Vector2.zero;
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
        uvs = new List<Vector2>();
        
        for (int i = 0; i < CHUNK_WIDTH; i++) {
            for (int j = 0; j < CHUNK_HEIGHT; j++) {
                for (int k = 0; k < CHUNK_LENGTH; k++) {
                    
                    this.setBlock(0, i, j, k, false);
        
                    int worldX = (int)(i + (CHUNK_WIDTH * position.x));
                    int worldY = j;
                    int worldZ = (int)(k + (CHUNK_LENGTH * position.y));
        
        
                    //float rand = applet.pow(j / ((float)CHUNK_HEIGHT) * 4, 1.1024f) + fastNoise.GetValueFractal(worldX * frequency, j * frequency, worldZ * frequency) * 0.60f;
                    //if (rand > 0.5f)
                    //{
                    if(k == 0 && i  == 0 && j == 0)
                        this.setBlock( 1 , i, j, k, false);
                        
                        // else
                        // {
                        //     this.setBlock(Random.Range(0f, 1f) < 0.1 ? 1 : 0, i, j, k, false);
                        // }
                        //}

                        //if(j == 254){
                        //if(applet.random(1) >= .5f){
                        //this.setBlock(Blocks.BEDROCK, i, j, k, false);
                        //}
                        //}

                        //if(j == 255){
                        //this.setBlock(Blocks.BEDROCK, i, j, k, false);
                        //}
                }
        
            }
        }


        // for (int i = 0; i < CHUNK_WIDTH; i++) {
        //     for (int k = 0; k < CHUNK_LENGTH; k++) {
        //
        //         for (int j = 0; j < CHUNK_HEIGHT; j++) {
        //             if(getBlock(i,j,k) == Blocks.AIR){
        //                 continue;
        //             }else{
        //                 setBlock(Blocks.GRASS, i, j, k, false);
        //                 setBlock(Blocks.DIRT, i, j + 1, k, false);
        //                 setBlock(Blocks.DIRT, i, j + 2, k, false);
        //                 setBlock(Blocks.DIRT, i, j + 3, k, false);
        //                 if(random.nextInt(10) < 2)
        //                 setBlock(Blocks.PLANT_GRASS, i, j - 1, k, false);
        //                 break;
        //             }
        //         }
        //
        //     }
        // }
        Mesh m = GetMesh();
        
        //vertices.AddRange(blockVerts);
        
        //triangles.AddRange(blockIndices);
        
        
        
        regenerate();

        
        SetMesh(ref m);
        
    }
    
    public void setBlock(int block, int x, int y, int z, bool setByUser) {
        int chunkLocation = y / cwidth;
        int blockLocation = y % cwidth;

        if(x < 0 || y < 0 || z < 0 || x > cwidth - 1 || y > 256 - 1 || z > clength - 1){
            return;
        }

        int[,,] blocks = data;//.get(chunkLocation);

        blocks[x, blockLocation, z] = block;

        //Determine if block in subChunk is neighboring another chunk and rebuild it too...
        // if (blockLocation == cheight - 1 && chunkLocation < chunkStackHeight - 1) {
        //     subChunkDirtyList[chunkLocation + 1] = true;
        // }
        // if (blockLocation == 0 && chunkLocation > 0) {
        //     subChunkDirtyList[chunkLocation - 1] = true;
        // }
        // if (subChunkDirtyList[chunkLocation] == false) {
        //     subChunkDirtyList[chunkLocation] = true;
        // }

        // If the block was placed by a user, update the neighboring chunks if on the edge of a chunk.

        // if (x == 0) {
        //     Chunk nx = world.getChunk(this.position.x - 1, this.position.y);
        //     if(nx != null) {
        //         if (Blocks.IsSolid(nx.getBlock(cwidth - 1, y, z))) {
        //
        //             nx.subChunkDirtyList[chunkLocation] = true;
        //             //nx.regenerate();
        //         }
        //     }
        // }
        // if (x == cwidth - 1) {
        //     Chunk nx = world.getChunk(this.position.x + 1, this.position.y);
        //     if(nx != null) {
        //         if (Blocks.IsSolid(nx.getBlock(0, y, z))) {
        //             nx.subChunkDirtyList[chunkLocation] = true;
        //             //nx.regenerate();
        //         }
        //     }
        // }
        // if (z == 0) {
        //     Chunk nz = world.getChunk(this.position.x, this.position.y - 1);
        //     if(nz != null) {
        //         if (Blocks.IsSolid(nz.getBlock(x, y, clength - 1))) {
        //             nz.subChunkDirtyList[chunkLocation] = true;
        //             //nz.regenerate();
        //         }
        //     }
        // }
        // if (z == clength - 1) {
        //     Chunk nz = world.getChunk(this.position.x, this.position.y + 1);
        //     if(nz != null) {
        //         if (Blocks.IsSolid(nz.getBlock(x, y, 0))) {
        //             nz.subChunkDirtyList[chunkLocation] = true;
        //             //nz.regenerate();
        //         }
        //     }
        // }

    }
    
    public int getBlock(int x, int y, int z) {
        int chunkLocation = y / cwidth;
        int blockLocation = y % cwidth;
        return data[x, blockLocation, z];
    }
    
    public void regenerate() {
        for (int i = 0; i < chunkStackHeight; i++) {
            for (int x = 0; x < cwidth; x++) {
                //for (int y = 0 + (i * cheight); y < cheight + (i * cheight); y++) {
                for (int y = 0; y < cheight; y++) {
                    for (int z = 0; z < clength; z++) {

                        if (getBlock(x, y, z) == 0){ //|| (!Blocks.IsOpaque(getBlock(x, y, z)) && Blocks.IsGlassLike(getBlock(x, y, z)))) {
                            continue;
                        }

                        Faces faces = new Faces();

                        if (x > 0)
                        {
                            faces.left = getBlock(x - 1, y, z) == 0;
                            //nx = !Blocks.IsSolid(getBlock(x - 1, y, z)) || !Blocks.IsOpaque(getBlock(x - 1, y, z));
                        }else
                        {
                            faces.left = true;
                            //nx = !Blocks.IsSolid(GetBlock(cnx,CHUNK_WIDTH - 1, y, z)) || !Blocks.IsOpaque(GetBlock(cnx,CHUNK_WIDTH - 1, y, z));
                        }

                        if (x < CHUNK_WIDTH - 1) {
                            faces.right = getBlock(x + 1, y, z) == 0;
                            //px = !Blocks.IsSolid(getBlock(x + 1, y, z)) || !Blocks.IsOpaque(getBlock(x + 1, y, z));
                        }else
                        {
                            faces.right = true;
                            //px = !Blocks.IsSolid(GetBlock(cpx,0, y, z)) || !Blocks.IsOpaque(GetBlock(cpx,0, y, z));
                        }

                        if (y > 0) {
                            faces.down = getBlock(x, y - 1, z) == 0;
                            //ny = !Blocks.IsSolid(getBlock(x, y - 1, z)) || !Blocks.IsOpaque(getBlock(x, y - 1, z));
                        }
                        else
                        {
                            faces.down = true;
                        }
                        

                        if (y < CHUNK_HEIGHT - 1) {
                            faces.up = getBlock(x, y + 1, z) == 0;
                            //py = !Blocks.IsSolid(getBlock(x, y + 1, z)) || !Blocks.IsOpaque(getBlock(x, y + 1, z));
                        }
                        else
                        {
                            faces.up = true;
                        }

                        

                        if (z > 0) {
                            faces.back = getBlock(x, y, z - 1) == 0;
                            //nz = !Blocks.IsSolid(getBlock(x, y, z - 1)) || !Blocks.IsOpaque(getBlock(x, y, z - 1));
                        }else
                        {
                            faces.back = true;
                            //nz = !Blocks.IsSolid(GetBlock(cny,x, y, CHUNK_LENGTH - 1))|| !Blocks.IsOpaque(GetBlock(cny,x, y, CHUNK_LENGTH - 1));
                        }

                        if (z < CHUNK_LENGTH - 1) {
                            faces.forward = getBlock(x, y, z + 1) == 0;
                            //pz = !Blocks.IsSolid(getBlock(x, y, z + 1)) || !Blocks.IsOpaque(getBlock(x, y, z + 1));
                        }else
                        {
                            faces.forward = true;
                            //pz = !Blocks.IsSolid(GetBlock(cpy,x, y, 0))|| !Blocks.IsOpaque(GetBlock(cpy,x, y, 0));
                        }



                        ChunkGeometry.ConstructBlock(getBlock(x,y,z), ref vertices, ref triangles, ref colors, ref uvs, faces, x,y,z);

                        //meshes[i].tint(this.applet.map(world.getBlock(x, y, z).getLightLevel(), 0, 15, 50, 255));

                    }
                }
            }
            
        }
    }

    private void SetMesh(ref Mesh m)
    {
        m.Clear();
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        m.colors = colors.ToArray();
        m.uv = uvs.ToArray();

        //m.RecalculateBounds();
        m.RecalculateNormals();
        m.RecalculateBounds();
        m.RecalculateTangents();

        GetComponent<Renderer>().material.DOFade(0, 0).OnComplete(() =>
        {

            GetComponent<Renderer>().material.DOFade(1f, 5f);

        });





}

    private Mesh GetMesh(){

        MeshFilter mf = this.GetComponent<MeshFilter>();
        if(mf == null){
            mf = this.gameObject.AddComponent<MeshFilter>();
            mf.mesh = new Mesh();
        }
        return mf.mesh;
    }
    
    


}
