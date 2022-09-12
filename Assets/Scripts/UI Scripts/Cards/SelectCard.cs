using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCard : MonoBehaviour
{
    public GameObject selectedCard;
    private IEnumerator coroutine;
    private IEnumerator coroutinePrev;
    private RectTransform cardTrans;
    private Vector3 newPos;
    private Color prevColor;
    private bool currentlyAnimating;
    [SerializeReference] ProjectileHandler projectileScript;
    [SerializeReference] SwitchStaff staffScript;

    private void Start()
    {
        prevColor = new Color32(140, 140, 140, 255);
        currentlyAnimating = false;
    }

    public void OnClick(GameObject cardClicked)
    {
        if (cardClicked != selectedCard && currentlyAnimating == false)
        {
            currentlyAnimating = true;
            if (selectedCard != null)
            {
                // First animate down the previous selected card
                RectTransform cardTrans = selectedCard.GetComponent<RectTransform>();
                Vector3 newPos = cardTrans.transform.localPosition + new Vector3(0, -40.0f, 0);
                coroutinePrev = moveSmoothly(selectedCard.GetComponent<Image>(), false, cardTrans.transform, cardTrans.transform.localPosition, newPos);
                StartCoroutine(coroutinePrev);
            }

            selectedCard = cardClicked;
            projectileScript.SwitchWeapon(cardClicked.name+"Projectile");
            staffScript.changeStaff(cardClicked.name + "Staff");
            // Now animate up the current selected card
            cardTrans = cardClicked.GetComponent<RectTransform>();
            newPos = cardTrans.transform.localPosition + new Vector3(0, 40.0f,0);
            coroutine = moveSmoothly(selectedCard.GetComponent<Image>(), true, cardTrans.transform, cardTrans.transform.localPosition, newPos);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator moveSmoothly(Image cardImg, bool selected, Transform tra, Vector3 from, Vector3 to)
    {
        var t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 3.0f;
            tra.localPosition = Vector3.Lerp(from, to, t);
            if (selected == true)
                cardImg.color = Color.Lerp(prevColor, Color.white, t);
            else
                cardImg.color = Color.Lerp(Color.white, prevColor, t);
            yield return null;
        }

        currentlyAnimating = false;
    }

}
