using System.Collections;
using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    public IEnumerator CreateBlock(Vector3 position)
    {
        float progress = 0;
        float step = 0.02f;
        Material material = GetComponent<Renderer>().material;
        Vector3 startPosition = position + new Vector3(0, 5, 0);
        material.color = new Color(0, 0.2f, 1, 0);
        while (progress <= 1)
        {
            transform.position = Vector3.Lerp(startPosition, position, progress);
            material.color += new Color(0, 0, 0, step);
            progress += step;
            yield return null;
        }
        transform.position = position;
        material.color = new Color(0, 0.2f, 1, 1);
        yield return null;
    }
    public IEnumerator DestroyBlock()
    {
        float progress = 0;
        float step = 0.02f;
        Material material = GetComponent<Renderer>().material;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition - new Vector3(0, 5, 0);
        while (progress <= 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            material.color -= new Color(0, 0, 0, step);
            progress += step;
            yield return null;
        }
        yield return null;
        Destroy(gameObject);
    }
}
