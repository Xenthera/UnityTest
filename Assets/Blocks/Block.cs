public class Block
{

    public enum BlockRenderType
    {
        BLOCK,
        SPRITE
    }

    public int lightLevel;

    public BlockSimpleMultiTexture texture;

    public BlockRenderType renderType;

    protected string name = "genericBlockName";

    public Block()
    {
        this.lightLevel = 15;
        this.renderType = BlockRenderType.BLOCK;
        name = this.ToString();
    }

    public virtual bool isSolid()
    {
        return true;
    }
    public virtual bool isOpaque()
    {
        return true;
    }

    public virtual string getName()
    {
        return this.name;
    }

    public virtual bool isAir()
    {
        return false;
    }

}