using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PulseScale : MonoBehaviour
{
    #region Variables
    [SerializeField] float pulseMultiplier = 2f;
    [SerializeField] float pulseSpeed = 2f;

    private bool growing = true;
    private Vector3 startScale;
    private Vector3 endScale;
    private Vector3 targetScale;
    #endregion
    private void Start()
    {
        startScale = transform.localScale;
        endScale = startScale * pulseMultiplier;
    }
    private void Update()
    {
        // Set growing (with padding space)
        if (transform.localScale.x >= endScale.x - 0.5) growing = false;
        else if (transform.localScale.x <= startScale.x + 0.5) growing = true;

        // set Target
        targetScale = growing ? endScale : startScale;

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, pulseSpeed * Time.deltaTime);
    }

}
