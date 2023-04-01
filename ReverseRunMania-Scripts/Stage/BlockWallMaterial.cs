using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWallMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTextureScale = new Vector2(3, 1);
    }
}
