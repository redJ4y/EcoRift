using UnityEngine;
using UnityEngine.UI;

public class healthBarScript : MonoBehaviour
{
    [SerializeField]
    private Health health;

    [SerializeField]
    private RectTransform barRect;

    [SerializeField]
    private RectMask2D mask;

    private float maxRightMask;
    private float initialRightMask;
    private void Start()
    {
        //x = left , w = top , y = bottom , z = right
        maxRightMask = barRect.rect.width - mask.padding.x - mask.padding.z;
        initialRightMask = mask.padding.z;
        Debug.Log("healthBarScript start method successfully called");
    }

    public void SetValue()
    {
      float currentHealth = health.GetHp();
      var targetWidth = currentHealth * maxRightMask / health.GetMaxHp();
      var newRightMask = maxRightMask + initialRightMask - targetWidth;
      var padding = mask.padding;
      padding.z = newRightMask;
      mask.padding = padding;
      Debug.Log("healthBarScript setValue successfully called");
    }
}
