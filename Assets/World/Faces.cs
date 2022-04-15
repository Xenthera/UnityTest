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

    public void reset()
    {
        this.up = false;
        this.down = false;
        this.left = false;
        this.right = false;
        this.forward = false;
        this.back = false;
    }
}
