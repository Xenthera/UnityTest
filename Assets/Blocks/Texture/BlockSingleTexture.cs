public class BlockSingleTexture : BlockSimpleMultiTexture
{
    public BlockSingleTexture(int indexX, int indexY) : base()
    {
        this.setBOTTOM(indexX, indexY);
        this.setSIDES(indexX, indexY);
        this.setTOP(indexX, indexY);
    }
}