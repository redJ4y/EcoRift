using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float hp;
    private float maxHp;

    private float barWidth;
    private float startXPos;
    private Transform hpBar;
    private SpriteRenderer hpRenderer;
    private SpriteRenderer innerHpRenderer;
    private GameObject actor;
    private volatile bool isFading;
    private volatile bool isVisible;
    private volatile bool justHit;

    // Start is called before the first frame update
    void Start()
    {
        isFading = false;
        isVisible = false;
        justHit = false;
        actor = transform.parent.gameObject;

        maxHp = hp;
        hpBar = transform.Find("InnerHealth");
        innerHpRenderer = hpBar.GetComponent<SpriteRenderer>();
        hpRenderer = transform.gameObject.GetComponent<SpriteRenderer>();

        if (hpBar == null)
        {
            Debug.Log("HPBar not found");
        }

        barWidth = hpBar.localScale.x;
        startXPos = hpBar.localPosition.x + (barWidth / 2);
    }

    void FixedUpdate()
    {
        if (hp >= 0.0f && hpBar != null)
        {
            float hpRatio = hp / maxHp;
            hpBar.localScale = new Vector3(hpRatio * barWidth, hpBar.localScale.y, 0);
            hpBar.localPosition = new Vector3(startXPos - (barWidth - (hpRatio * barWidth / 2)), hpBar.localPosition.y, hpBar.localPosition.z);
        }
    }

    private void Death()
    {
        Destroy(actor);
    }

    public void TakeDamage(float damage)
    {
        justHit = true;
        hp -= damage;

        if (hp <= 0.0f)
        {
            Death();
        }

        if (isFading == false)
        {
            isFading = true;

            if (isVisible == false)
            {

                StartCoroutine(fadeIn());
            }
            else
            {
                StartCoroutine(fadeOut());
            }
        }
    }

    private IEnumerator fadeOut()
    {

        while (justHit == true)
        {
            justHit = false;
            yield return new WaitForSeconds(3);

        }

        var t = 0f;
        Color prevColor = hpRenderer.color;
        Color tempColor = hpRenderer.color;
        tempColor.a = 0.0f;

        Color innerPrevColor = innerHpRenderer.color;
        Color innerTempColor = innerHpRenderer.color;
        innerTempColor.a = 0.0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 3.0f;

            innerHpRenderer.color = Color.Lerp(innerPrevColor, innerTempColor, t);
            hpRenderer.color = Color.Lerp(prevColor, tempColor, t);
            yield return null;
        }

        isVisible = false;
        isFading = false;
    }

    private IEnumerator fadeIn()
    {
        var t = 0f;
        Color prevColor = hpRenderer.color;
        Color tempColor = hpRenderer.color;
        tempColor.a = 1.0f;

        Color innerPrevColor = innerHpRenderer.color;
        Color innerTempColor = innerHpRenderer.color;
        innerTempColor.a = 1.0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 3.0f;

            innerHpRenderer.color = Color.Lerp(innerPrevColor, innerTempColor, t);
            hpRenderer.color = Color.Lerp(prevColor, tempColor, t);
            yield return null;
        }

        StartCoroutine(fadeOut());

        isVisible = true;
    }

    public float GetHp()
    {
      return hp;
      Debug.Log($"hp = {hp}");
    }

    public float GetMaxHp()
    {
      return maxHp;
      Debug.Log($"hp = {hp}");
    }

    public void lowerHP()
    {
      if(hp>=11)
      {
        hp-=10;
        Debug.Log($"hp = {hp}");
      }
    }

    public void raiseHP()
    {
      if(hp<=(maxHp-11))
      {
        hp+=10;
        Debug.Log($"hp = {hp}");
      }
    }
}
