//script to control player's health bar
using UnityEngine;
using UnityEngine.UI;

public class healthBarScript : MonoBehaviour
{
    [SerializeField]
    private Health health; // Player's health

    [SerializeField]
    private RectTransform barRect; // healthbar rect

    [SerializeField]
    private RectMask2D mask; // using mask to grow or shrink bar

    private float maxRightMask;
    private float initialRightMask;
    private void Start()
    {
        //x = left , w = top , y = bottom , z = right
        maxRightMask = barRect.rect.width - mask.padding.x - mask.padding.z;
        initialRightMask = mask.padding.z;
    }

    private void Update()
    {
        SetValue();
    }

    public void SetValue() // updates visual of healthbar according to player HP
    {
      float currentHealth = health.GetHp();
      var targetWidth = currentHealth * maxRightMask / health.GetMaxHp();
      var newRightMask = maxRightMask + initialRightMask - targetWidth;
      var padding = mask.padding;
      padding.z = newRightMask;
      mask.padding = padding;
    }
}
