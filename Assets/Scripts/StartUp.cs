using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartUp : MonoBehaviour
{
    [SerializeField]
    Material lightMaterial;
    [SerializeField]
    Material darkMaterial;

    [SerializeField]
    TilemapRenderer darkTilemap;
    [SerializeField]
    TilemapRenderer lightTilemap;
    // Start is called before the first frame update
    void Start()
    {
        darkTilemap.material = darkMaterial;
        lightTilemap.material = lightMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
