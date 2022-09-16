using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour
{
    Camera cam;
    public GameObject damageText;
    public float damage;
    public string element;
    private Collider2D thisCollider;
    private IEnumerator coroutine;
    private GetWeather getWeather;
    private bool damageEnemies = false;

    void Awake()
    {
        thisCollider = gameObject.GetComponent<Collider2D>();
        thisCollider.enabled = false;
        cam = Camera.main;
        getWeather = GameObject.Find("Backgrounds").GetComponent<GetWeather>();

        if (cam == null)
        {
            Debug.Log("Camera is null");
        }
    }

    public void SetIgnoreCollision(Collider2D collider, bool damageEnemies)
    {
        this.damageEnemies = damageEnemies;
        Physics2D.IgnoreCollision(thisCollider, collider);
        thisCollider.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }

        Vector2 endPos = col.contacts[0].point + new Vector2(1.5f, 0);

        //Vector3 endPos = thisCollider.bounds.center + new Vector3(gameObject.transform.position.x,0,0);

        GameObject target = col.gameObject;

        if (target.layer == 10 || target.layer == 9) // Check if collider is enemy or player
        {
            if (target.layer == 9 || damageEnemies)
                target.transform.Find("HealthBar").GetComponent<Health>().TakeDamage(15.0f);
            Destroy(gameObject);
        }
    }

    void DisplayDamage(Vector2 endPosition, float newDamage)
    {
        Transform damageTemp = GameObject.Find("DamageMarkers").transform;
        GameObject text = Instantiate(damageText, damageTemp);
        TMP_Text tmp = text.GetComponent<TMP_Text>();
        tmp.text = "-" + newDamage;

        // Positioning
        Vector2 textPos = cam.WorldToScreenPoint(endPosition);
        Transform textTrans = text.GetComponent<RectTransform>().transform;
        textTrans.position = textPos;
        Vector3 endTextPos = textTrans.position + new Vector3(0, 20.0f, 0);

        // Animation
        coroutine = moveSmoothly(tmp, textTrans, textTrans.position, endTextPos);
        CoroutineManager.Instance.StartCoroutine(coroutine);
        Destroy(text, 1.5f);
        Destroy(gameObject);
    }

    private IEnumerator moveSmoothly(TMP_Text tmp, Transform tra, Vector3 from, Vector3 to)
    {
        var t = 0f;

        while (t < 1.5f)
        {
            t += Time.deltaTime;
            tmp.fontMaterial.SetColor("_FaceColor", Color.Lerp(Color.white, Color.clear, t));
            tra.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

    private float InflictDamage(float damage, GameObject target)
    {
        float newDamage = damage;
        string weather = getWeather.getWeatherType();

        string buffed = "Nothing";

        switch (weather)
        {
            case "Clear":
                buffed = "Sunny";
                break;
            case "Clouds":
                buffed = "Storm";
                break;
            case "Rain":
                buffed = "Rain";
                break;
        }

        if (buffed == element)
        {
            newDamage *= 2f;
        }

        //target.GetComponent<Health>().hp -= newDamage;
        return newDamage;
    }
}
