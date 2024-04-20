using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilateAnim : MonoBehaviour
{
    private Vector3 startpos;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private float amplitude = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = new Vector3(startpos.x, startpos.y + amplitude * Mathf.Sin(Time.timeSinceLevelLoad * 8 + delay), startpos.z);
            
    }
}
