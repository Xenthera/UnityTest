using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using static Chunk;
using Random = UnityEngine.Random;

public class World : Singleton<World>
{
    // Start is called before the first frame update

    public int chunkWidth = 2, chunkLength = 2;

    Dictionary<(int, int), Chunk> chunks;

    public GameObject Chunk;
    public float NoiseScale = 0.9f;

    public Queue<LightNode> lightBfsQueue;

    public List<Vector4> debugCubes;


    protected override void Awake()
    {
        debugCubes = new List<Vector4>();
        chunks = new Dictionary<(int, int), Chunk>();
        lightBfsQueue = new Queue<LightNode>();

        Random.seed = 25;  

        Sequence s = DOTween.Sequence();

        float t1 = Time.realtimeSinceStartup;

        for (int i = 0; i < chunkWidth; i++)
        {
            for (int j = 0; j < chunkLength; j++)
            {
                int x = i;
                int y = j;
                //s.AppendCallback(() =>
                //{

                    GameObject chunk = Instantiate(Chunk);
                    Chunk c = chunk.GetComponent<Chunk>();
                    c.position.x = x;
                    c.position.y = y;
                    chunks.TryAdd((x, y), c);

                    chunk.transform.position = new Vector3(c.position.x * 16, 0, c.position.y * 16);

                //}).AppendInterval(0.01f);
                

            }
        }

        float t2 = Time.realtimeSinceStartup;

        Bobby.Log("Time to make chunks: " + (t2 - t1) + " seconds");
    }


    void Start()
    {
        foreach (var chunk in chunks)
        {
            chunk.Value.gameObject.transform.position = new Vector3(chunk.Value.position.x * 16, 0, chunk.Value.position.y * 16);
            
        }


    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    private void OnDrawGizmos()
    {
        foreach (var pos in debugCubes)
        {
            Gizmos.color = new Color(pos.w / 16f, pos.w / 16f, pos.w / 16f);
            Gizmos.DrawWireCube(new Vector3(pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f), Vector3.one * 0.5f);
        }
    }

    public void UpdateLight()
    {
        while (lightBfsQueue.Count > 0)
        {
            LightNode l = lightBfsQueue.Dequeue();
            Vector3 index = l.index;
            int x = (int)index.x;
            int y = (int)index.y;
            int z = (int)index.z;
            Chunk c = l.chunk;
            byte lightR = c.getLightLevelR(x, y, z);
            byte lightG = c.getLightLevelG(x, y, z);
            byte lightB = c.getLightLevelB(x, y, z);
            
            
           
            if (x == 0)
            {
                Chunk negX = World.Instance.getChunk((int)(c.position.x - 1), (int)c.position.y);
                if (negX != null)
                {
                    bool qother = false;

                    if (negX.getBlock(CHUNK_WIDTH - 1, y, z) == 0 && negX.getLightLevelR(CHUNK_WIDTH - 1, y, z) + 2 <= lightR)
                    {
                        negX.setLightLevelR((byte)(lightR - 1), CHUNK_WIDTH - 1, y, z);
                        qother = true;

                    }
                    if (negX.getBlock(CHUNK_WIDTH - 1, y, z) == 0 && negX.getLightLevelR(CHUNK_WIDTH - 1, y, z) - 2 >= lightR)
                    {

                        //c.setLightLevelR((byte)(negX.getLightLevelR(CHUNK_WIDTH - 1, y, z) - 1), x, y, z);
                        qother = true;

                    }

                    if (negX.getBlock(CHUNK_WIDTH - 1, y, z) == 0 && negX.getLightLevelG(CHUNK_WIDTH - 1, y, z) + 2 <= lightG)
                    {
                        negX.setLightLevelG((byte)(lightG - 1), CHUNK_WIDTH - 1, y, z);
                        qother = true;

                    }
                    else if (negX.getBlock(CHUNK_WIDTH - 1, y, z) == 0 && negX.getLightLevelG(CHUNK_WIDTH - 1, y, z) - 2 >= lightG)
                    {

                        //c.setLightLevelG((byte)(negX.getLightLevelG(CHUNK_WIDTH - 1, y, z) - 1), x, y, z);
                        qother = true;


                    }

                    if (negX.getBlock(CHUNK_WIDTH - 1, y, z) == 0 && negX.getLightLevelB(CHUNK_WIDTH - 1, y, z) + 2 <= lightB)
                    {
                        negX.setLightLevelB((byte)(lightB - 1), CHUNK_WIDTH - 1, y, z);
                        qother = true;
                        
                    }
                    else if (negX.getBlock(CHUNK_WIDTH - 1, y, z) == 0 && negX.getLightLevelB(CHUNK_WIDTH - 1, y, z) - 2 >= lightB)
                    {

                        //c.setLightLevelB((byte)(negX.getLightLevelB(CHUNK_WIDTH - 1, y, z) - 1), x, y, z);
                        qother = true;


                    }

                    if (qother)
                    {
                        lightBfsQueue.Enqueue(new LightNode(new Vector3(CHUNK_WIDTH - 1, y, z), negX));
                    }

                    negX.markDirty(CHUNK_WIDTH - 1, y, z);
                }
            }
            else
            {

                bool qthis = false;

                if (c.getBlock(x - 1, y, z) == 0 && c.getLightLevelR(x - 1, y, z) + 2 <= lightR)
                {
                    c.setLightLevelR((byte)(lightR - 1), x - 1, y, z);
                    qthis = true;
                }

                if (c.getBlock(x - 1, y, z) == 0 && c.getLightLevelG(x - 1, y, z) + 2 <= lightG)
                {
                    c.setLightLevelG((byte)(lightG - 1), x - 1, y, z);
                    qthis = true;
                }
                if (c.getBlock(x - 1, y, z) == 0 && c.getLightLevelB(x - 1, y, z) + 2 <= lightB)
                {
                    c.setLightLevelB((byte)(lightB - 1), x - 1, y, z);
                    qthis = true;
                }

                if (qthis)
                {
                    lightBfsQueue.Enqueue(new LightNode(new Vector3(x - 1, y, z), c));
                }
            }

            if (x == CHUNK_WIDTH - 1)
            {
                Chunk posX = World.Instance.getChunk((int)(c.position.x + 1), (int)c.position.y);
                if (posX != null)
                {
                    bool qother = false;

                    if (posX.getBlock(0, y, z) == 0 && posX.getLightLevelR(0, y, z) + 2 <= lightR)
                    {
                        posX.setLightLevelR((byte)(lightR - 1), 0, y, z);
                        qother = true;
                    }
                    else if(posX.getBlock(0, y, z) == 0 && posX.getLightLevelR(0, y, z) - 2 >= lightR)
                    {
                        
                        //c.setLightLevelR((byte)(posX.getLightLevelR(0, y, z) - 1), x, y, z);
                        qother = true;
                    }

                    if (posX.getBlock(0, y, z) == 0 && posX.getLightLevelG(0, y, z) + 2 <= lightG)
                    {
                        posX.setLightLevelG((byte)(lightG - 1), 0, y, z);
                        qother = true;
                    }
                    else if (posX.getBlock(0, y, z) == 0 && posX.getLightLevelG(0, y, z) - 2 >= lightG)
                    {

                        //c.setLightLevelG((byte)(posX.getLightLevelG(0, y, z) - 1), x, y, z);
                        qother = true;
                    }

                    if (posX.getBlock(0, y, z) == 0 && posX.getLightLevelB(0, y, z) + 2 <= lightB)
                    {
                        posX.setLightLevelB((byte)(lightB - 1), 0, y, z);
                        qother = true;
                    }
                    else if (posX.getBlock(0, y, z) == 0 && posX.getLightLevelB(0, y, z) - 2 >= lightB)
                    {

                        //c.setLightLevelB((byte)(posX.getLightLevelB(0, y, z) - 1), x, y, z);
                        qother = true;
                    }

                    if (qother)
                    {
                        lightBfsQueue.Enqueue(new LightNode(new Vector3(0, y, z), posX));
                    }



                    posX.markDirty(0, y, z);
                }
            }
            else
            {

                bool qthis = false;

                if (c.getBlock(x + 1, y, z) == 0 && c.getLightLevelR(x + 1, y, z) + 2 <= lightR)
                {
                    c.setLightLevelR((byte)(lightR - 1), x + 1, y, z);
                    qthis = true;

                }
                if (c.getBlock(x + 1, y, z) == 0 && c.getLightLevelG(x + 1, y, z) + 2 <= lightG)
                {
                    c.setLightLevelG((byte)(lightG - 1), x + 1, y, z);
                    qthis = true;

                }
                if (c.getBlock(x + 1, y, z) == 0 && c.getLightLevelB(x + 1, y, z) + 2 <= lightB)
                {
                    c.setLightLevelB((byte)(lightB - 1), x + 1, y, z);
                    qthis = true;

                }

                if (qthis)
                {
                    lightBfsQueue.Enqueue(new LightNode(new Vector3(x + 1, y, z), c));
                }
            }

            if (z == 0)
            {
                Chunk negZ = World.Instance.getChunk((int)(c.position.x), (int)c.position.y - 1);
                if (negZ != null)
                {
                    bool qother = false;

                    if (negZ.getBlock(x, y, CHUNK_LENGTH - 1) == 0 && negZ.getLightLevelR(x, y, CHUNK_LENGTH - 1) + 2 <= lightR)
                    {
                        negZ.setLightLevelR((byte)(lightR - 1), x, y, CHUNK_LENGTH - 1);
                        qother = true;
                    }
                    else if (negZ.getBlock(x, y, CHUNK_LENGTH - 1) == 0 && negZ.getLightLevelR(x, y, CHUNK_LENGTH - 1) - 2 >= lightR)
                    {

                        //c.setLightLevelR((byte)(negZ.getLightLevelR(x, y, CHUNK_LENGTH - 1) - 1), x, y, z);
                        if(c.position.x == 1 && c.position.y == 2 && y < 48)
                            Bobby.Log("test");
                        qother = true;
                        

                    }
                    if (negZ.getBlock(x, y, CHUNK_LENGTH - 1) == 0 && negZ.getLightLevelG(x, y, CHUNK_LENGTH - 1) + 2 <= lightG)
                    {
                        negZ.setLightLevelG((byte)(lightG - 1), x, y, CHUNK_LENGTH - 1);
                        qother = true;
                    }
                    else if (negZ.getBlock(x, y, CHUNK_LENGTH - 1) == 0 && negZ.getLightLevelG(x, y, CHUNK_LENGTH - 1) - 2 >= lightG)
                    {

                        //c.setLightLevelG((byte)(negZ.getLightLevelG(x, y, CHUNK_LENGTH - 1) - 1), x, y, z);
                        qother = true;

                    }

                    if (negZ.getBlock(x, y, CHUNK_LENGTH - 1) == 0 && negZ.getLightLevelB(x, y, CHUNK_LENGTH - 1) + 2 <= lightB)
                    {
                        negZ.setLightLevelB((byte)(lightB - 1), x, y, CHUNK_LENGTH - 1);
                        qother = true;
                    }
                    else if (negZ.getBlock(x, y, CHUNK_LENGTH - 1) == 0 && negZ.getLightLevelB(x, y, CHUNK_LENGTH - 1) - 2 >= lightB)
                    {

                        //c.setLightLevelB((byte)(negZ.getLightLevelB(x, y, CHUNK_LENGTH - 1) - 1), x, y, z);
                        qother = true;

                    }
                    if (qother)
                    {
                        lightBfsQueue.Enqueue(new LightNode(new Vector3(x, y, CHUNK_LENGTH - 1), negZ));
                    }


                    negZ.markDirty(x, y, CHUNK_LENGTH - 1);
                }
            }
            else
            {
                bool qthis = false;
                if (c.getBlock(x, y, z - 1) == 0 && c.getLightLevelR(x, y, z - 1) + 2 <= lightR)
                {
                    c.setLightLevelR((byte)(lightR - 1), x, y, z - 1);
                    qthis = true;
                }
                if (c.getBlock(x, y, z - 1) == 0 && c.getLightLevelG(x, y, z - 1) + 2 <= lightG)
                {
                    c.setLightLevelG((byte)(lightG - 1), x, y, z - 1);
                    qthis = true;
                }
                if (c.getBlock(x, y, z - 1) == 0 && c.getLightLevelB(x, y, z - 1) + 2 <= lightB)
                {
                    c.setLightLevelB((byte)(lightB - 1), x, y, z - 1);
                    qthis = true;
                }

                if (qthis)
                {
                    lightBfsQueue.Enqueue(new LightNode(new Vector3(x, y, z - 1), c));
                }
            }

            if (z == CHUNK_LENGTH - 1)
            {
                
                    Chunk posZ = World.Instance.getChunk((int)(c.position.x), (int)c.position.y + 1);
                    if (posZ != null)
                    {
                        bool qother = false;

                        if (posZ.getBlock(x, y, 0) == 0 && posZ.getLightLevelR(x, y, 0) + 2 <= lightR)
                        {
                            posZ.setLightLevelR((byte)(lightR - 1), x, y, 0);
                            qother = true;
                        }
                        else if (posZ.getBlock(x, y, 0) == 0 && posZ.getLightLevelR(x, y, 0) - 2 >= lightR)
                        {

                            //c.setLightLevelR((byte)(posZ.getLightLevelR(x, y, 0) - 1), x, y, z);
                            qother = true;
                        }
                        if (posZ.getBlock(x, y, 0) == 0 && posZ.getLightLevelG(x, y, 0) + 2 <= lightG)
                        {
                            posZ.setLightLevelG((byte)(lightG - 1), x, y, 0);
                            qother = true;
                        }
                        else if (posZ.getBlock(x, y, 0) == 0 && posZ.getLightLevelG(x, y, 0) - 2 >= lightG)
                        {

                            //c.setLightLevelG((byte)(posZ.getLightLevelG(x, y, 0) - 1), x, y, z);
                            qother = true;
                        }
                        if (posZ.getBlock(x, y, 0) == 0 && posZ.getLightLevelB(x, y, 0) + 2 <= lightB)
                        {
                            posZ.setLightLevelB((byte)(lightB - 1), x, y, 0);
                            qother = true;
                        }
                        else if (posZ.getBlock(x, y, 0) == 0 && posZ.getLightLevelB(x, y, 0) - 2 >= lightB)
                        {

                            //c.setLightLevelB((byte)(posZ.getLightLevelB(x, y, 0) - 1), x, y, z);
                            qother = true;
                        }

                        if (qother)
                        {
                            lightBfsQueue.Enqueue(new LightNode(new Vector3(x, y, 0), posZ));
                        }

                        posZ.markDirty(x, y, 0);
                    }
                
            }
            else
            {
                bool qthis = false;
                if (c.getBlock(x, y, z + 1) == 0 && c.getLightLevelR(x, y, z + 1) + 2 <= lightR)
                {
                    c.setLightLevelR((byte)(lightR - 1), x, y, z + 1);
                    qthis = true;
                }

                if (c.getBlock(x, y, z + 1) == 0 && c.getLightLevelG(x, y, z + 1) + 2 <= lightG)
                {
                    c.setLightLevelG((byte)(lightG - 1), x, y, z + 1);
                    qthis = true;
                }
                if (c.getBlock(x, y, z + 1) == 0 && c.getLightLevelB(x, y, z + 1) + 2 <= lightB)
                {
                    c.setLightLevelB((byte)(lightB - 1), x, y, z + 1);
                    qthis = true;
                }

                if (qthis)
                {
                    lightBfsQueue.Enqueue(new LightNode(new Vector3(x, y, z + 1), c));
                }
            }

            bool qy = false;

            if (y > 0 && c.getBlock(x, y - 1, z) == 0 && c.getLightLevelR(x, y - 1, z) + 2 <= lightR)
            {
                c.setLightLevelR((byte)(lightR - 1), x, y - 1, z);
                qy = true;
            }
            else
            {
                c.markDirty(x, y-1, z);
            }

            if (y > 0 && c.getBlock(x, y - 1, z) == 0 && c.getLightLevelG(x, y - 1, z) + 2 <= lightG)
            {
                c.setLightLevelG((byte)(lightG - 1), x, y - 1, z);
                qy = true;
            }
            else
            {
                c.markDirty(x, y-1, z);
            }

            if (y > 0 && c.getBlock(x, y - 1, z) == 0 && c.getLightLevelB(x, y - 1, z) + 2 <= lightB)
            {
                c.setLightLevelB((byte)(lightB - 1), x, y - 1, z);
                qy = true;
            }
            else
            {
                c.markDirty(x, y-1, z);
            }

            if (qy)
            {
                lightBfsQueue.Enqueue(new LightNode(new Vector3(x, y - 1, z), c));
            }

            qy = false;

            if (y < CHUNK_HEIGHT - 1 && c.getBlock(x, y + 1, z) == 0 && c.getLightLevelR(x, y + 1, z) + 2 <= lightR)
            {
                c.setLightLevelR((byte)(lightR - 1), x, y + 1, z);
                qy = true;
            }


            if (y < CHUNK_HEIGHT - 1 && c.getBlock(x, y + 1, z) == 0 && c.getLightLevelG(x, y + 1, z) + 2 <= lightG)
            {
                c.setLightLevelG((byte)(lightG - 1), x, y + 1, z);
                qy = true;
            }

            if (y < CHUNK_HEIGHT - 1 && c.getBlock(x, y + 1, z) == 0 && c.getLightLevelB(x, y + 1, z) + 2 <= lightB)
            {
                c.setLightLevelB((byte)(lightB - 1), x, y + 1, z);
                qy = true;
            }


            if (qy)
            {
                lightBfsQueue.Enqueue(new LightNode(new Vector3(x, y + 1, z), c));
            }
        }
    }

    public Chunk getChunk(int chunkX, int chunkZ)
    {
        try
        {
            return chunks[(chunkX, chunkZ)];
        }
        catch
        {
            return null;
        }

    }

    public int getBlock(int x, int y, int z)
    {
        int chunkX = x / 16;
        int blockX = x % 16;
        int chunkZ = z / 16;
        int blockZ = z % 16;

        int chunkIndex = chunkX * chunkWidth + chunkZ;

        Chunk c = getChunk(chunkX, chunkZ);

        if (c != null)
        {
            return c.getBlock(blockX, y, blockZ);
        }
        else
        {
            return Blocks.AIR;
        }

    }

    public void setBlock(short block, int x, int y, int z, bool setByUser)
    {

        int chunkX = x / 16;
        int blockX = x % 16;
        int chunkZ = z / 16;
        int blockZ = z % 16;

        Chunk c = getChunk(chunkX, chunkZ);

        if (c != null)
        {
            c.setBlock(block, blockX, y, blockZ, setByUser);
        }
    }


    public static float Perlin3D(float x, float y, float z)
    {
        float ab = Mathf.PerlinNoise(x, y);
        float bc = Mathf.PerlinNoise(y, z);
        float ac = Mathf.PerlinNoise(x, z);

        float ba = Mathf.PerlinNoise(y, x);
        float cb = Mathf.PerlinNoise(z, y);
        float ca = Mathf.PerlinNoise(z, x);

        float abc = ab + bc + ac + ba + cb + ca;

        return abc / 6f;
    }
}
