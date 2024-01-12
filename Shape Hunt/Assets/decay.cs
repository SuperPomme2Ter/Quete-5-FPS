using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decay : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 dir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y<0)
        {
            Destroy(gameObject);
        }
    }
}
