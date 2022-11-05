using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLighting : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] RenderTexture lightTexture;
    [SerializeField] GameObject player;
    [SerializeField] Camera cam;
    [SerializeField] int radius;

    private Texture2D currentTexture;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        currentTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
    }

    private void Update()
    {
        Vector3Int start = tilemap.WorldToCell(player.transform.position - new Vector3(radius, radius, 0));
        Vector3Int offset = tilemap.WorldToCell(player.transform.position + new Vector3(radius, radius, 0)) - start;

        for (int xPos = start.x; xPos < start.x + offset.x; xPos++)
        {
            for (int yPos = start.y; yPos < start.y + offset.y; yPos++)
            {
                Vector3Int posVector3Int = new Vector3Int(xPos, yPos, 0);

                if (tilemap.GetTile(posVector3Int)?.GetType() == typeof(BackgroundTile))
                {
                    BackgroundTile tile = (BackgroundTile)tilemap.GetTile(posVector3Int);
                    if (tile.isNonChangeable)
                    {
                        continue;
                    }
                }

                Vector3 pos = tilemap.CellToWorld(posVector3Int);
                Vector3 screenPos = cam.WorldToScreenPoint(pos);
                int xOff = Mathf.RoundToInt((screenPos.x / cam.pixelWidth) * lightTexture.width);
                int yOff = Mathf.RoundToInt((screenPos.y / cam.pixelHeight) * lightTexture.height);

                RenderTexture.active = lightTexture;
                currentTexture.ReadPixels(new Rect(xOff, yOff, 1, 1), 0, 0);
                currentTexture.Apply();

                Color color = currentTexture.GetPixel(0, 0);
                //Debug.Log("r:" + color.r + " g:" + color.g + " b:" + color.b);
                if (color.r >= 0.6f && color.g >= 0.6f && color.b >= 0.6f)
                {
                    tilemap.SetColliderType(posVector3Int, Tile.ColliderType.Sprite);
                } else
                {                    
                    tilemap.SetColliderType(posVector3Int, Tile.ColliderType.None);
                }
                
            }
        }
    }

}
