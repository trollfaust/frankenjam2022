using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLighting : MonoBehaviour
{
    [SerializeField] RenderTexture lightTexture;
    Camera cam;
    [SerializeField] AFFECTEDSTATE affectedState;
    [SerializeField] Collider2D colliderToChange;
    [SerializeField] LightingColor lightingColor;

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
            Color color = lightingColor.GetColorFromWordPos(pos);

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
