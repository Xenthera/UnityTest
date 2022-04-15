public class BlockAir : Block
{

    public BlockAir() : base()
    {
        name = "Air";
    }

    public override bool isSolid()
    {
        return false;
    }
    public override bool isOpaque() { return false; }


    public override bool isAir()
    {
        return true;
    }
}