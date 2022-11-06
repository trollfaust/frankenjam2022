using UnityEngine;

public class LightingColor : MonoBehaviour
{
    Camera cam;
    [SerializeField] RenderTexture lightTexture;
    private Texture2D currentTexture;

    private void Awake()
    {
        cam = Camera.main;
        currentTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
    }

    public Color GetColorFromWordPos(Vector3 pos)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(pos);
        int xOff = Mathf.RoundToInt((screenPos.x / cam.pixelWidth) * lightTexture.width);
        int yOff = Mathf.RoundToInt((screenPos.y / cam.pixelHeight) * lightTexture.height);
        if (xOff < 0 || xOff >= lightTexture.width || yOff < 0 || yOff >= lightTexture.height)
        {
            return Color.red;
        }

        RenderTexture.active = lightTexture;
        currentTexture.ReadPixels(new Rect(xOff, yOff, 1, 1), 0, 0);
        currentTexture.Apply();

        return currentTexture.GetPixel(0, 0);
    }
}
