
public class Tuple
{
    public int x, y;
    public Tuple(int x, int y)
    {
        this.x = x;
        this.y = y;
    }


    public override bool Equals(object obj)
    {
        if(obj == null)
        {
            return false;
        }

        Tuple t = obj as Tuple;
        return t.x == x && t.y == y;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

}