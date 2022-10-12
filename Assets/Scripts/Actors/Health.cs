using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private int hpRegenRate = 5;
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
    public ParticleSystem hitParticles;

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

        if (actor.Equals(GameObject.FindWithTag("Player")))
        {
            StartCoroutine(RegenHealth());
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

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(0);
    }

    private void Death()
    {
        if (actor.layer == 9)
            StartCoroutine(GoToMainMenu());
        else
            Destroy(actor);
    }


    private IEnumerator RegenHealth()
    {
        while (true)
        {
            if(hp < maxHp)
            {
                hp += hpRegenRate;
                yield return new WaitForSeconds(5);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        emitParticles(); // emit blood particles on hit
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
    }

    public float GetMaxHp()
    {
      return maxHp;
    }

    //debug tool for testing healthbar
    public void lowerHP()
    {
      if(hp>=11)
      {
        hp-=10;
      }
    }

    //debug tool for testing healthbar
    public void raiseHP()
    {
      if(hp<=(maxHp-11))
      {
        hp+=10;
      }
    }

    //blood particle emission
    void emitParticles(){
      hitParticles.Play();
    }

    public void buffHp(float multiplier)
    {
        hp *= multiplier;
    }
}
