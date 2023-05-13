using UnityEditor;
using UnityEngine;

public class ResizableRects
{
    public Rect basicRect;
    public Rect resizeRect;

    public ResizableRects(float x, float y, float width, float height)
    {
        basicRect = new Rect(x, y, width, height);
        resizeRect = new Rect(x + width, y, 1f, height);
    }
}