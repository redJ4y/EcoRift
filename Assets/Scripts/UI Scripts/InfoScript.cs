using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoScript : MonoBehaviour
{
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private Vector3 endingPos;
    [SerializeField] private float duration;
    [SerializeField] private float animationSpeed = 2.0f;
    [SerializeReference] private GameObject textObj;

    public void Alert(string message)
    {
        GameObject newText = Instantiate(textObj, gameObject.transform);
        TMP_Text text = newText.GetComponent<TMP_Text>();
        text.text = message;
        StartCoroutine(moveSmoothly(text, newText.transform));
    }

    private IEnumerator moveSmoothly(TMP_Text tmp, Transform tra)
    {
        var t = 0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime * animationSpeed;
            tmp.fontMaterial.SetColor("_FaceColor", Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), Color.white, t));
            tra.localPosition = Vector3.Lerp(startingPos, endingPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * animationSpeed;
            tmp.fontMaterial.SetColor("_FaceColor", Color.Lerp(Color.white, new Color(1.0f, 1.0f, 1.0f, 0.0f), t));
            yield return null;
        }

        Destroy(tra.gameObject);
    }
}
