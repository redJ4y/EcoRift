using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDataScript : MonoBehaviour
{
    [SerializeReference] private GameObject confirmationCanvas;
    [SerializeReference] private CompletionEmblems emblemScript;

    public void ResetData()
    {
        Debug.Log("Reset");
        DataManager.Instance.ResetData();
        confirmationCanvas.SetActive(false);
        emblemScript.updateEmblems(); 
    }

    public void ShowConfirmationCanvas()
    {
        confirmationCanvas.SetActive(true);
    }

    public void HideConfirmationCanvas()
    {
        Debug.Log("hide ");
        confirmationCanvas.SetActive(false);
    }
}
