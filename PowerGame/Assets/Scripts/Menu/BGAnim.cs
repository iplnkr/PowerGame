using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAnim : MonoBehaviour
{
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        rend.material.SetTextureOffset("_MainTex", new Vector2(Time.timeSinceLevelLoad/5f, -Time.timeSinceLevelLoad/9f));
    }
}
