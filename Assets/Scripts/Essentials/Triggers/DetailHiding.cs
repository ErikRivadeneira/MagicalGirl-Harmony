using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DetailHiding : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] float fadeRate = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            tilemap.color = new Color(1, 1, 1, 0f);
        }
        
        /*
        StartCoroutine(FadeToTransparent());*/
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            tilemap.color = new Color(1, 1, 1, 1f);
        }
        /*
        StopCoroutine(FadeToTransparent());*/
    }

    IEnumerator FadeToTransparent()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha = alpha - Time.deltaTime * fadeRate;
            tilemap.color = new Color(1,1,1, alpha);
            yield return null;
        }
        yield return null;
    }
    IEnumerator FadeToWhite()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha = alpha - Time.deltaTime * fadeRate;
            tilemap.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
        yield return null;
    }
}
