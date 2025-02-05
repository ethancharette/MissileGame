using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnStart : MonoBehaviour
{
    [SerializeField] float scaleMin = 1.0f;
    [SerializeField] float scaleMax = 3.0f;
    private Vector3 targetScale;
    private Vector3 currentScale;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = transform.localScale * Random.Range(scaleMin, scaleMax);
        targetScale = transform.localScale;
        currentScale = transform.localScale = new Vector3(0.1f,0.1f,0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScale.x < targetScale.x)
        {
            currentScale = Vector3.Lerp(currentScale, targetScale, 1.5f * Time.deltaTime);
            transform.localScale = currentScale;
        }
        else
        {
            Destroy(this);
        }
    }
}
