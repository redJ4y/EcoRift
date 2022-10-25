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
    private float movementAmount;

    [SerializeReference] ProjectileHandler projectileScript;
    [SerializeReference] SwitchStaff staffScript;
    [SerializeReference] SpinTier tierScript;

    private void Start()
    {
        movementAmount = 40f;
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
                Vector3 newPos = cardTrans.transform.localPosition + new Vector3(0, -movementAmount, 0);
                coroutinePrev = moveSmoothly(selectedCard.GetComponent<Image>(), false, cardTrans.transform, cardTrans.transform.localPosition, newPos);
                StartCoroutine(coroutinePrev);
            }

            selectedCard = cardClicked;
            projectileScript.SwitchWeapon(cardClicked.name+"Projectile1");
            tierScript.SetNewWeather(cardClicked.name);

            staffScript.changeStaff(cardClicked.name + "Staff");
            // Now animate up the current selected card
            cardTrans = cardClicked.GetComponent<RectTransform>();
            newPos = cardTrans.transform.localPosition + new Vector3(0, movementAmount, 0);
            coroutine = moveSmoothly(selectedCard.GetComponent<Image>(), true, cardTrans.transform, cardTrans.transform.localPosition, newPos);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator moveSmoothly(Image cardImg, bool selected, Transform tra, Vector3 from, Vector3 to)
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * 3.0f;
            tra.localPosition = Vector3.Lerp(from, to, time);

            if (selected == true)
                cardImg.color = Color.Lerp(prevColor, Color.white, time);
            else
                cardImg.color = Color.Lerp(Color.white, prevColor, time);
            yield return null;
        }

        currentlyAnimating = false;
    }
}
