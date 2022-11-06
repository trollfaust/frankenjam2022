using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum AFFECTEDSTATE { Light, Dark }
public class TilemapLighting : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] RenderTexture lightTexture;
    GameObject player;
    Camera cam;
    [SerializeField] int radius;
    [SerializeField] AFFECTEDSTATE affectedState;
    [SerializeField] LightingColor lightingColor;

    private Texture2D currentTexture;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = player.GetComponentInChildren<Camera>();
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
                Vector3Int gridPos = new Vector3Int(xPos, yPos, 0);

                if (tilemap.GetTile(gridPos)?.GetType() == typeof(BackgroundTile))
                {
                    BackgroundTile tile = (BackgroundTile)tilemap.GetTile(gridPos);
                    if (tile.isNonChangeable)
                    {
                        continue;
                    }
                }

                Vector3 pos = tilemap.CellToWorld(gridPos);

                Color color = lightingColor.GetColorFromWordPos(pos);
                //Debug.Log("r:" + color.r + " g:" + color.g + " b:" + color.b);
                if (color.r >= 0.5f && color.g >= 0.5f && color.b >= 0.5f)
                {
                    ChangeCollider(gridPos, true);
                } else
                {
                    ChangeCollider(gridPos, false);
                }
                
            }
        }
    }

    private void ChangeCollider(Vector3Int gridPos, bool change)
    {
        if ((affectedState == AFFECTEDSTATE.Dark && !change) || (affectedState == AFFECTEDSTATE.Light && change))
        {
            tilemap.SetColliderType(gridPos, Tile.ColliderType.Sprite);
        } else
        {
            tilemap.SetColliderType(gridPos, Tile.ColliderType.None);
        }
    }

}
