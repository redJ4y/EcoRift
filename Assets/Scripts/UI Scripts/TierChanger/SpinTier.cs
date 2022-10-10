using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTier : MonoBehaviour
{
    [SerializeField] private float animationSpeed;
    private bool currentlyAnimating;

    // Start is called before the first frame update
    void Start()
    {
        currentlyAnimating = false;
    }

    public void SpinRevolver()
    {
        if (!currentlyAnimating)
            StartCoroutine(SpinAnimation());
    }

    private IEnumerator SpinAnimation()
    {
        currentlyAnimating = true;
        float currentTime = 0f;
        float currentAngle = transform.eulerAngles.z;
        float targetAngle = currentAngle - 120f;

        Debug.Log("Clicked");

        while (currentAngle >= targetAngle)
        {
            currentTime += Time.deltaTime * animationSpeed;
            transform.eulerAngles = new Vector3(0.0f, 0.0f, currentAngle);
            currentAngle -= 10f;
            yield return null;
        }

        currentlyAnimating = false;
    }
}
