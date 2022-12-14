using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private int hpRegenRate = 5;
    [SerializeField] private HealthBarScript healthBarScript;
    public float maxHp;

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
    private bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        actor = transform.parent.gameObject;
        if (actor != null)
        {
            isFading = false;
            isVisible = false;
            justHit = false;

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
                isPlayer = true;
                StartCoroutine(RegenHealth());
            }

            barWidth = hpBar.localScale.x;
            startXPos = hpBar.localPosition.x + (barWidth / 2);
        }
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
            if (hp < maxHp)
            {
                if((hp + hpRegenRate) > maxHp)
                {
                    hp = maxHp;
                }
                else
                {
                    hp += hpRegenRate;
                }

                if (healthBarScript != null)
                    healthBarScript.SetValue();
                yield return new WaitForSeconds(5);
            }
            else
            {
                yield return new WaitForSeconds(2);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        EmitParticles(); // emit blood particles on hit
        justHit = true;
        hp -= damage;

        if (isPlayer)
        {
            healthBarScript.SetValue();
        }

        if (hp <= 0.0f)
        {
            Death();
        }

        if (isFading == false)
        {
            isFading = true;

            if (isVisible == false)
            {

                StartCoroutine(FadeIn());
            }
            else
            {
                StartCoroutine(FadeOut());
            }
        }
    }

    private IEnumerator FadeOut()
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

    private IEnumerator FadeIn()
    {
        var time = 0f;
        Color prevColor = hpRenderer.color;
        Color tempColor = hpRenderer.color;
        tempColor.a = 1.0f; // set the opacity to 100%

        Color innerPrevColor = innerHpRenderer.color;
        Color innerTempColor = innerHpRenderer.color;
        innerTempColor.a = 1.0f; // set the opacity to 100%

        while (time < 1f)
        {
            time += Time.deltaTime * 3.0f;

            innerHpRenderer.color = Color.Lerp(innerPrevColor, innerTempColor, time);
            hpRenderer.color = Color.Lerp(prevColor, tempColor, time);
            yield return null;
        }

        StartCoroutine(FadeOut());

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
    public void LowerHP()
    {
        if (hp >= 11)
        {
            hp -= 10;
        }
    }

    //debug tool for testing healthbar
    public void RaiseHP()
    {
        hp += 10;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
    }

    //blood particle emission
    public void EmitParticles()
    {
        if (hitParticles)
        {
            hitParticles.Play();
        }
    }

    public void BuffHp(float multiplier)
    {
        float oldMaxHp = maxHp;
        maxHp *= multiplier;
        hp += maxHp - oldMaxHp;
    }

    public void PrepareForTest()
    {
        isFading = true; // block fading
        hp = 100.0f;
        maxHp = hp;
        StartCoroutine(RegenHealth());
    }
}
