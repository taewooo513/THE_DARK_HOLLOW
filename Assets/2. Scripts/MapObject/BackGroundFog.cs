using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundFog : MonoBehaviour
{
    public float speed;
    public float directionX;

    private float spritewidth;
    float x = 0;

    // Start is called before the first frame update
    void Start()
    {
        spritewidth = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (directionX == 1)
        {
            if (x > spritewidth)
            {
                x = 0;
            }
        }
        else
        {
            if (x < -spritewidth)
            {
                x = 0;
            }
        }

        Debug.Log(spritewidth);
        x += Time.deltaTime * speed * directionX;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
