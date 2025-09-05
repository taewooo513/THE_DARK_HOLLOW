using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    private SpriteRenderer renderer;
    public float timeAlpha;
    private float alpha = 1;
    public float randPowerMaxX;
    public float randPowerMinX;
    public float randPowerMaxY;
    public float randPowerMinY;

    private void Awake()
    {
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void Init(Vector2 dir)
    {
        StartCoroutine("AlphaScale");
        dir *= new Vector2(Random.Range(randPowerMinX, randPowerMaxX), Random.Range(randPowerMinY, randPowerMaxY));
        gameObject.GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);
    }

    IEnumerator AlphaScale()
    {
        while (true)
        {
            if (alpha <= 0)
                break;

            alpha -= Time.deltaTime;
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
            Vector3 scale = transform.localScale;
            scale.x = alpha;
            scale.y = alpha;

            yield return null;
        }
        Destroy(this.gameObject);
    }
}
