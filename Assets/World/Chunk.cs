using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Schema;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Sequence = DG.Tweening.Sequence;


public class Chunk : MonoBehaviour
{

    public struct LightNode
    {
        public Vector3 index;
        public Chunk chunk;
        public LightNode(Vector3 indx, Chunk chunk)
        {
            this.index = indx;
            this.chunk = chunk;
        }
    }

    public static FastNoise fastNoise;

    static Chunk(){
        fastNoise = new FastNoise();
    }

    public GameObject SubChunk;
    public Material material = null;
    public Vector2 position;



    public const int CHUNK_WIDTH = 16, CHUNK_LENGTH = 16, CHUNK_HEIGHT = 128;



    private const int cwidth = 16, clength = 16, cheight = 16;
    private const int chunkStackHeight = CHUNK_HEIGHT / cheight; // 16 * 8 = 128;

    
    
   

    List<SubChunk> subChunks;

    bool[] subChunkDirtyList;

    Color[] colors = new Color[] { new Color(1, 1, 0), new Color(0, 1, 0) };//, new Color(0, 0, 1)};




    private void Awake()
    {
        subChunks = new List<SubChunk>();
        subChunkDirtyList = new bool[chunkStackHeight];

       

        for (int i = 0; i < chunkStackHeight; i++)
        {
            GameObject subChunk = Instantiate(SubChunk);
            subChunk.transform.parent = gameObject.transform;

            subChunks.Add(subChunk.GetComponent<SubChunk>());
            subChunkDirtyList[i] = true;
        }
    }


    void Start()
    {
        


        for (int i = 0; i < CHUNK_WIDTH; i++) {
            for (int j = 0; j < CHUNK_HEIGHT; j++) {
                for (int k = 0; k < CHUNK_LENGTH; k++) {
                    
                    //setBlock(0, i, j, k, false);
        
                    int worldX = (int)(i + (CHUNK_WIDTH * position.x));
                    int worldY = j;
                    int worldZ = (int)(k + (CHUNK_LENGTH * position.y));

                    float frequency = 4.0f;



                    //float rand = fastNoise.GetValueFractal(worldX * frequency, worldY * frequency, worldZ * frequency) * 0.6f;
                    float thresh = World.Perlin3D(worldX * World.Instance.NoiseScale, worldY * World.Instance.NoiseScale, worldZ * World.Instance.NoiseScale);
                    if (thresh > 0.5f)
                    {
                        //Vector3 a = new Vector3(worldX, worldY, worldZ);
                        //Vector3 b = new Vector3((World.Instance.chunkLength * 16) / 2f, CHUNK_HEIGHT / 2f, (World.Instance.chunkLength * 16) / 2f);
                        //if (Vector3.Distance(a, b) < 64)
                        {
                            //if (worldX == 0 && worldY == 0)
                            if (worldY < 64)
                                setBlock(1, i, j, k, false);
                        }
                    }

                    if(worldX == 0 || worldX == 127 || worldZ == 0 || worldZ == 127 || worldY == 0)
                    {
                        if (worldY < 64)
                            setBlock(1, i, j, k, false);
                    }
                    
                    if(worldY == 64)
                    {
                        if (worldX % 2 == 0 && worldZ % 2 == 1)
                        {
                            setBlock(1, i, j, k, false);
                        }
                    }
                        //setBlock(1, i, j, k, false);


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


        for (int i = 0; i < CHUNK_WIDTH; i++)
        {
            for (int j = 0; j < CHUNK_HEIGHT; j++)
            {
                for (int k = 0; k < CHUNK_LENGTH; k++)
                {
                    int worldX = (int)(i + (CHUNK_WIDTH * position.x));
                    int worldY = j;
                    int worldZ = (int)(k + (CHUNK_LENGTH * position.y));

                    int block = getBlock(i, j, k);

                    //if (block == Blocks.AIR)
                    //{
                    //    setLightLevelR(0, i, j, k);
                    //    setLightLevelG(0, i, j, k);
                    //    setLightLevelB(0, i, j, k);
                    //    World.Instance.lightBfsQueue.Enqueue(new LightNode(new Vector3(i, j, k), this));
                    //}

                    //if (block == Blocks.AIR)
                    //{
                    //    if (worldX == 64 - 8 && worldY == 64 && worldZ == 64 - 8)
                    //    {
                    //        setLightLevelR(0, i, j, k);
                    //        setLightLevelG(0, i, j, k);
                    //        setLightLevelB(16, i, j, k);
                    //        World.Instance.lightBfsQueue.Enqueue(new LightNode(new Vector3(i, j, k), this));
                    //    }
                    //    if (worldX == 64 && worldY == 64 && worldZ == 64 - 8)
                    //    {
                    //        setLightLevelR(16, i, j, k);
                    //        setLightLevelG(0, i, j, k);
                    //        setLightLevelB(0, i, j, k);
                    //        World.Instance.lightBfsQueue.Enqueue(new LightNode(new Vector3(i, j, k), this));
                    //    }
                    //    if (worldX == 64 - 8 && worldY == 64 && worldZ == 64)
                    //    {
                    //        setLightLevelR(0, i, j, k);
                    //        setLightLevelG(16, i, j, k);
                    //        setLightLevelB(0, i, j, k);
                    //        World.Instance.lightBfsQueue.Enqueue(new LightNode(new Vector3(i, j, k), this));
                    //    }
                    //}

                    if (block == Blocks.AIR)
                    {
                        setLightLevelR(0, i, j, k);
                        setLightLevelG(0, i, j, k);
                        setLightLevelB(0, i, j, k);
                        World.Instance.lightBfsQueue.Enqueue(new LightNode(new Vector3(i, j, k), this));
                    }

                    if (j < 66 && Random.Range(0f, 1f) < 0.0005f)
                    {

                        if (block == Blocks.AIR)
                        {

                            Color c = colors[(int)Random.Range(0, colors.Length)];

                            setLightLevelR((byte)((byte)c.r * 16), i, j, k);
                            setLightLevelG((byte)((byte)c.g * 16), i, j, k);
                            setLightLevelB((byte)((byte)c.b * 16), i, j, k);
                            World.Instance.lightBfsQueue.Enqueue(new LightNode(new Vector3(i, j, k), this));

                        }

                    }
                    else
                    {

                    }



                }
            }
        }



        World.Instance.UpdateLight();



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



    }

    private bool canSeeSky(int x, int y, int z)
    {
        for (int i = y + 1; i < CHUNK_HEIGHT; i++)
        {
            if (getBlock(x, i, z) != 0)
            {
                return false;
            }
        }
        return true;
    }

    private bool isDirty()
    {
        for (int i = 0; i < subChunkDirtyList.Length; i++)
        {
            if (subChunkDirtyList[i] == true)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (isDirty())
        {
            regenerate();
        }
    }

    public void setBlock(int block, int x, int y, int z, bool setByUser) {
        int chunkLocation = y / cwidth;
        int blockLocation = y % cwidth;

        if(x < 0 || y < 0 || z < 0 || x > cwidth - 1 || y > 256 - 1 || z > clength - 1){
            return;
        }

        int[,,] blocks = subChunks[chunkLocation].data;

        blocks[x, blockLocation, z] = block;

        //Determine if block in subChunk is neighboring another chunk and rebuild it too...
        if (blockLocation == cheight - 1 && chunkLocation < chunkStackHeight - 1)
        {
            subChunkDirtyList[chunkLocation + 1] = true;
        }
        if (blockLocation == 0 && chunkLocation > 0)
        {
            subChunkDirtyList[chunkLocation - 1] = true;
        }
        if (subChunkDirtyList[chunkLocation] == false)
        {
            subChunkDirtyList[chunkLocation] = true;
        }
        

        if (x == 0)
        {
            Chunk nx = World.Instance.getChunk((int)position.x - 1, (int)position.y);
            if (nx != null)
            {
                
                if (Blocks.IsSolid(nx.getBlock(cwidth - 1, y, z)))
                {

                    nx.subChunkDirtyList[chunkLocation] = true;
                    //nx.regenerate();
                }
            }
        }
        if (x == cwidth - 1)
        {
            Chunk nx = World.Instance.getChunk((int)position.x + 1, (int)position.y);
            if (nx != null)
            {
                if (Blocks.IsSolid(nx.getBlock(0, y, z)))
                {
                    nx.subChunkDirtyList[chunkLocation] = true;
                    //nx.regenerate();
                }
            }
        }
        if (z == 0)
        {    

            Chunk nz = World.Instance.getChunk((int)position.x, (int)position.y - 1);



                if (nz != null)
            {
                if (Blocks.IsSolid(nz.getBlock(x, y, clength - 1)))
                {


                    nz.subChunkDirtyList[chunkLocation] = true;
                    //nz.regenerate();
                }
            }
        }
        if (z == clength - 1)
        {

            Chunk nz = World.Instance.getChunk((int)position.x, (int)position.y + 1);
            if (nz != null)
            {

                if (Blocks.IsSolid(nz.getBlock(x, y, 0)))
                {
                    nz.subChunkDirtyList[chunkLocation] = true;
                    //nz.regenerate();
                }
            }
        }

    }
    
    public int getBlock(int x, int y, int z) {
        int chunkLocation = y / cheight;
        int blockLocation = y % cheight;
        return subChunks[chunkLocation].data[x, blockLocation, z];
    }

    public byte getLightLevelR(int x, int y, int z)
    {
        int chunkLocation = y / cheight;
        int blockLocation = y % cheight;

        if (x == -1)
        {
            Chunk nx = World.Instance.getChunk((int)position.x - 1, (int)position.y);
            if (nx != null)
            {
                return nx.getLightLevelR(cwidth - 1, y, z);
            }
            else
            {
                return 16;
            }
        }
        if (x == cwidth)
        {
            Chunk nx = World.Instance.getChunk((int)position.x + 1, (int)position.y);
            if (nx != null)
            {
                return nx.getLightLevelR(0, y, z);
            }
            else
            {
                return 16;
            }
        }
        if (z == -1)
        {
            Chunk nz = World.Instance.getChunk((int)position.x, (int)position.y - 1);
            if (nz != null)
            {
                return nz.getLightLevelR(x, y, clength - 1);
            }
            else
            {
                return 16;
            }
        }
        if (z == clength)
        {
            Chunk nz = World.Instance.getChunk((int)position.x, (int)position.y + 1);
            if (nz != null)
            {
                return nz.getLightLevelR(x, y, 0);
            }
            else
            {
                return 16;
            }
        }

        if (y == -1)
        {
            return 4;
        }

        if (y == CHUNK_HEIGHT)
        {
            return 16;
        }

        return subChunks[chunkLocation].lightInfoR[x, blockLocation, z];
    }

    public byte getLightLevelG(int x, int y, int z)
    {
        int chunkLocation = y / cheight;
        int blockLocation = y % cheight;

        if (x == -1)
        {
            Chunk nx = World.Instance.getChunk((int)position.x - 1, (int)position.y);
            if (nx != null)
            {
                return nx.getLightLevelG(cwidth - 1, y, z);
            }
            else
            {
                return 16;
            }
        }
        if (x == cwidth)
        {
            Chunk nx = World.Instance.getChunk((int)position.x + 1, (int)position.y);
            if (nx != null)
            {
                return nx.getLightLevelG(0, y, z);
            }
            else
            {
                return 16;
            }
        }
        if (z == -1)
        {
            Chunk nz = World.Instance.getChunk((int)position.x, (int)position.y - 1);
            if (nz != null)
            {
                return nz.getLightLevelG(x, y, clength - 1);
            }
            else
            {
                return 16;
            }
        }
        if (z == clength)
        {
            Chunk nz = World.Instance.getChunk((int)position.x, (int)position.y + 1);
            if (nz != null)
            {
                return nz.getLightLevelG(x, y, 0);
            }
            else
            {
                return 16;
            }
        }

        if (y == -1)
        {
            return 4;
        }

        if (y == CHUNK_HEIGHT)
        {
            return 16;
        }

        return subChunks[chunkLocation].lightInfoG[x, blockLocation, z];
    }

    public byte getLightLevelB(int x, int y, int z)
    {
        int chunkLocation = y / cheight;
        int blockLocation = y % cheight;

        if (x == -1)
        {
            Chunk nx = World.Instance.getChunk((int)position.x - 1, (int)position.y);
            if (nx != null)
            {
                return nx.getLightLevelB(cwidth - 1, y, z);
            }
            else
            {
                return 16;
            }
        }
        if (x == cwidth)
        {
            Chunk nx = World.Instance.getChunk((int)position.x + 1, (int)position.y);
            if (nx != null)
            {
                return nx.getLightLevelB(0, y, z);
            }
            else
            {
                return 16;
            }
        }
        if (z == -1)
        {
            Chunk nz = World.Instance.getChunk((int)position.x, (int)position.y - 1);
            if (nz != null)
            {
                return nz.getLightLevelB(x, y, clength - 1);
            }
            else
            {
                return 16;
            }
        }
        if (z == clength)
        {
            Chunk nz = World.Instance.getChunk((int)position.x, (int)position.y + 1);
            if (nz != null)
            {
                return nz.getLightLevelB(x, y, 0);
            }
            else
            {
                return 16;
            }
        }

        if (y == -1)
        {
            return 4;
        }

        if (y == CHUNK_HEIGHT)
        {
            return 16;
        }

        return subChunks[chunkLocation].lightInfoB[x, blockLocation, z];
    }
    public void setLightLevelR(byte level, int posx, int posy, int posz)
    {
        int chunkLocation = posy / cheight;
        int blockLocation = posy % cheight;
        subChunks[chunkLocation].lightInfoR[posx, blockLocation, posz] = level;

        subChunkDirtyList[chunkLocation] = true;

    }

    public void setLightLevelG(byte level, int posx, int posy, int posz)
    {
        int chunkLocation = posy / cheight;
        int blockLocation = posy % cheight;
        subChunks[chunkLocation].lightInfoG[posx, blockLocation, posz] = level;

        subChunkDirtyList[chunkLocation] = true;

    }

    public void setLightLevelB(byte level, int posx, int posy, int posz)
    {
        int chunkLocation = posy / cheight;
        int blockLocation = posy % cheight;
        subChunks[chunkLocation].lightInfoB[posx, blockLocation, posz] = level;

        subChunkDirtyList[chunkLocation] = true;

    }

    public void markDirty(int posx, int posy, int posz)
    {
        int chunkLocation = posy / cheight;
        subChunkDirtyList[chunkLocation] = true;

    }

    public static int TryGetBlock(Chunk c, int x, int y, int z)
    {
        if (c != null)
        {
            return c.getBlock(x, y, z);
        }
        else
        {
            return Blocks.AIR;
        }
    }

    public void regenerate() {

        

        Faces faces = new Faces();

        Chunk chunkNegX = World.Instance.getChunk((int)position.x - 1, (int)position.y);
        Chunk chunkPosX = World.Instance.getChunk((int)position.x + 1, (int)position.y);

        Chunk chunkNegY = World.Instance.getChunk((int)position.x, (int)position.y - 1);
        Chunk chunkPosY = World.Instance.getChunk((int)position.x, (int)position.y + 1);

        for (int i = 0; i < chunkStackHeight; i++) 
        {
            SubChunk s = subChunks[i];

            if (subChunkDirtyList[i])
            {
                s.Vertices.Clear();
                s.Triangles.Clear();
                s.Colors.Clear();
                s.Uvs.Clear();
                for (int x = 0; x < cwidth; x++)
                {
                    for (int y = 0 + (i * cheight); y < cheight + (i * cheight); y++)
                    {
                        for (int z = 0; z < clength; z++)
                        {


                            


                            if (getBlock(x, y, z) == 0)
                            { //|| (!Blocks.IsOpaque(getBlock(x, y, z)) && Blocks.IsGlassLike(getBlock(x, y, z)))) {
                                continue;
                            }

                            faces.reset();

                            if (x > 0)
                            {
                                faces.left = getBlock(x - 1, y, z) == 0;
                                //nx = !Blocks.IsSolid(getBlock(x - 1, y, z)) || !Blocks.IsOpaque(getBlock(x - 1, y, z));
                            }
                            else
                            {
                                //faces.left = true;
                                faces.left = !Blocks.IsSolid(TryGetBlock(chunkNegX, CHUNK_WIDTH - 1, y, z)) || !Blocks.IsOpaque(TryGetBlock(chunkNegX, CHUNK_WIDTH - 1, y, z));
                            }

                            if (x < CHUNK_WIDTH - 1)
                            {
                                faces.right = getBlock(x + 1, y, z) == 0;
                                //px = !Blocks.IsSolid(getBlock(x + 1, y, z)) || !Blocks.IsOpaque(getBlock(x + 1, y, z));
                            }
                            else
                            {
                                faces.right = !Blocks.IsSolid(TryGetBlock(chunkPosX, 0, y, z)) || !Blocks.IsOpaque(TryGetBlock(chunkPosX, 0, y, z));
                                //px = !Blocks.IsSolid(GetBlock(cpx,0, y, z)) || !Blocks.IsOpaque(GetBlock(cpx,0, y, z));
                            }

                            if (y > 0)
                            {
                                faces.down = getBlock(x, y - 1, z) == 0;
                                //ny = !Blocks.IsSolid(getBlock(x, y - 1, z)) || !Blocks.IsOpaque(getBlock(x, y - 1, z));
                            }
                            else
                            {
                                faces.down = true;
                            }


                            if (y < CHUNK_HEIGHT - 1)
                            {
                                faces.up = getBlock(x, y + 1, z) == 0;
                                //py = !Blocks.IsSolid(getBlock(x, y + 1, z)) || !Blocks.IsOpaque(getBlock(x, y + 1, z));
                            }
                            else
                            {
                                faces.up = true;
                            }



                            if (z > 0)
                            {
                                faces.back = getBlock(x, y, z - 1) == 0;
                                //nz = !Blocks.IsSolid(getBlock(x, y, z - 1)) || !Blocks.IsOpaque(getBlock(x, y, z - 1));
                            }
                            else
                            {
                                faces.back = !Blocks.IsSolid(TryGetBlock(chunkNegY, x, y, CHUNK_LENGTH - 1)) || !Blocks.IsOpaque(TryGetBlock(chunkNegY, x, y, CHUNK_LENGTH - 1));
                                //nz = !Blocks.IsSolid(GetBlock(cny,x, y, CHUNK_LENGTH - 1))|| !Blocks.IsOpaque(GetBlock(cny,x, y, CHUNK_LENGTH - 1));
                            }

                            if (z < CHUNK_LENGTH - 1)
                            {
                                faces.forward = getBlock(x, y, z + 1) == Blocks.AIR;
                                //pz = !Blocks.IsSolid(getBlock(x, y, z + 1)) || !Blocks.IsOpaque(getBlock(x, y, z + 1));
                            }
                            else
                            {
                                
                                faces.forward = !Blocks.IsSolid(TryGetBlock(chunkPosY, x, y, 0)) || !Blocks.IsOpaque(TryGetBlock(chunkPosY, x, y, 0));
                                //pz = !Blocks.IsSolid(GetBlock(cpy,x, y, 0))|| !Blocks.IsOpaque(GetBlock(cpy,x, y, 0));
                            }



                            ChunkGeometry.ConstructBlock(getBlock(x, y, z), this, ref s, faces, x, y, z);

                           

                            //meshes[i].tint(this.applet.map(world.getBlock(x, y, z).getLightLevel(), 0, 15, 50, 255));

                        }
                    }
                }
                subChunkDirtyList[i] = false;
            }
            s.SetMesh();
        }
    }

    
    
    


}
