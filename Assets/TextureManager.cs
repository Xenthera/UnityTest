public class TextureManager
{

    public static TextureAtlasPosition getTextureIndex(int[] pos)
    {
        float texturePerRow = 16;
        float indvTexSize = 1f / texturePerRow;


        float xMin = (pos[0] * indvTexSize);
        float yMin = (pos[1] * indvTexSize);

        float xMax = (xMin + indvTexSize);
        float yMax = (yMin + indvTexSize);
        return new TextureAtlasPosition(xMin, yMin, xMax, yMax);
    }
}