using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLighting : MonoBehaviour
{
    [SerializeField] RenderTexture lightTexture;
    Camera cam;
    [SerializeField] AFFECTEDSTATE affectedState;
    [SerializeField] Collider2D colliderToChange;

    private Texture2D currentTexture;

    private void Awake()
    {
        cam = Camera.main;
        currentTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
    }

    private void Update()
    {
        Vector3[] positions = new Vector3[4] {
            new Vector3(transform.position.x - 1, transform.position.y - 0.5f, transform.position.z),
            new Vector3(transform.position.x - 1, transform.position.y + 0.5f, transform.position.z),
            new Vector3(transform.position.x + 1, transform.position.y - 0.5f, transform.position.z),
            new Vector3(transform.position.x + 1, transform.position.y + 0.5f, transform.position.z)
        };

        foreach (Vector3 pos in positions)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(pos);
            if (screenPos.x < 0 || screenPos.x >= cam.pixelWidth || screenPos.y < 0 || screenPos.y >= cam.pixelHeight)
            {
                continue;
            }
            int xOff = Mathf.RoundToInt((screenPos.x / cam.pixelWidth) * lightTexture.width);
            int yOff = Mathf.RoundToInt((screenPos.y / cam.pixelHeight) * lightTexture.height);
            if (xOff < 0 || xOff >= lightTexture.width || yOff < 0 || yOff >= lightTexture.height)
            {
                continue;
            }

            RenderTexture.active = lightTexture;
            currentTexture.ReadPixels(new Rect(xOff, yOff, 1, 1), 0, 0);
            currentTexture.Apply();

            Color color = currentTexture.GetPixel(0, 0);
            if (color.r >= 0.5f && color.g >= 0.5f && color.b >= 0.5f)
            {
                ChangeCollider(true);
            }
            else
            {
                ChangeCollider(false);
            }
        }
    }

    private void ChangeCollider(bool change)
    {
        if ((affectedState == AFFECTEDSTATE.Dark && !change) || (affectedState == AFFECTEDSTATE.Light && change))
        {
            colliderToChange.enabled = true;
        }
        else
        {
            colliderToChange.enabled = false;
        }
    }
}
