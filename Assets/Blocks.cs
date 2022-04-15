using System.Collections.Generic;

public class Blocks
{

    //region Block Classes
    //private static readonly BlockAir BLOCK_AIR = new BlockAir();
    //private static readonly BlockBedrock BLOCK_BEDROCK = new BlockBedrock();
    //private static readonly BlockBirch BLOCK_BIRCH = new BlockBirch();
    //private static readonly BlockBirchLeaves BLOCK_BIRCH_LEAVES = new BlockBirchLeaves();
    //private static readonly BlockDeadShrub BLOCK_DEAD_SHRUB = new BlockDeadShrub();
    //private static readonly BlockDirt BLOCK_DIRT  = new BlockDirt();
    //private static readonly BlockFlower BLOCK_FLOWER  = new BlockFlower();
    //private static readonly BlockGrass BLOCK_GRASS = new BlockGrass();
    //private static readonly BlockGravel BLOCK_GRAVEL = new BlockGravel();
    //private static readonly BlockOak BLOCK_OAK = new BlockOak();
    //private static readonly BlockOakLeaves BLOCK_OAK_LEAVES = new BlockOakLeaves();
    //private static readonly BlockPlantGrass BLOCK_PLANT_GRASS = new BlockPlantGrass();
    //private static readonly BlockReed BLOCK_REED = new BlockReed();
    //private static readonly BlockSand BLOCK_SAND = new BlockSand();
    //private static readonly BlockSandstone BLOCK_SANDSTONE = new BlockSandstone();
    //private static readonly BlockStone BLOCK_STONE = new BlockStone();
    //private static readonly BlockStoneBrick BLOCK_STONE_BRICK = new BlockStoneBrick();
    //private static readonly BlockOakPlanks BLOCK_OAK_PLANKS = new BlockOakPlanks();
    //private static readonly BlockGlass BLOCK_GLASS = new BlockGlass();
    //private static readonly BlockWater BLOCK_WATER = new BlockWater();
    //endregion

    public static readonly short AIR = 0;
    public static readonly short BEDROCK = 1;
    public static readonly short BIRCH = 2;
    public static readonly short BIRCH_LEAVES = 3;
    public static readonly short DEAD_SHRUB = 4;
    public static readonly short DIRT = 5;
    public static readonly short FLOWER = 6;
    public static readonly short GRASS = 7;
    public static readonly short GRAVEL = 8;
    public static readonly short OAK = 9;
    public static readonly short OAK_LEAVES = 10;
    public static readonly short PLANT_GRASS = 11;
    public static readonly short REED = 12;
    public static readonly short SAND = 13;
    public static readonly short SANDSTONE = 14;
    public static readonly short STONE = 15;
    public static readonly short STONE_BRICK = 16;
    public static readonly short OAK_PLANKS = 17;
    public static readonly short GLASS = 18;
    public static readonly short WATER = 19;

    public static readonly Dictionary<int, Block> BLOCKS;

    static Blocks()
    {
        BLOCKS = new Dictionary<int, Block>();
        BLOCKS.Add(AIR, new BlockAir());
        BLOCKS.Add(BEDROCK, new BlockStone());
    }

    public static bool IsSolid(int blockID)
    {
        return BLOCKS[blockID].isSolid();
    }
    public static bool IsAir(int blockID)
    {
        return BLOCKS[blockID].isAir();
    }
    public static bool IsOpaque(int blockID)
    {
        return BLOCKS[blockID].isOpaque();
    }


    public static Block.BlockRenderType RenderType(int blockID)
    {
        return BLOCKS[blockID].renderType;
    }

    public static BlockSimpleMultiTexture Texture(int blockID)
    {
        return BLOCKS[blockID].texture;
    }

}