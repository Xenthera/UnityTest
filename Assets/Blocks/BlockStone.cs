public class BlockStone : Block
{

    public BlockStone() : base()
    {
        this.name = "Stone";
        texture = new BlockSingleTexture(1, 0);
    }
}