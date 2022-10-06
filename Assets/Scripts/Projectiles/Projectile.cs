using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour
{
    Camera cam;
    [SerializeField] private string element;
    [SerializeField] private bool projectileDisabled;
    [SerializeField] public bool isBuffed;
    [SerializeField] public bool isRotatable;

    [SerializeField] private float damage;
    [Range(1f, 30f)] [SerializeField] public float bulletSpeed;
    [Range(0f, 15f)] [SerializeField] public float lifeSpan;
    [Range(1, 3)] [SerializeField] public int tier;

    private Collider2D thisCollider;
    private IEnumerator coroutine;
    private bool damageEnemies = false;

    void Awake()
    {
        thisCollider = gameObject.GetComponent<Collider2D>();
        cam = Camera.main;

        if (cam == null)
        {
            Debug.Log("Camera is null");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!projectileDisabled)
        {
            if (col.gameObject.layer == 8) // If collides with ground
            {
                Destroy(gameObject);
            }

            Vector2 endPos = col.contacts[0].point + new Vector2(1.5f, 0);

            //Vector3 endPos = thisCollider.bounds.center + new Vector3(gameObject.transform.position.x,0,0);

            GameObject target = col.gameObject;

            if (target.layer == 10 || target.layer == 9) // Check if collider is enemy or player
            {
                if (isBuffed)
                    damage *= 1.2f;
                target.transform.Find("HealthBar").GetComponent<Health>().TakeDamage(damage);
                CharacterController2D targetController = target.GetComponent<CharacterController2D>();
                FlyingCharacterController2D flyingTargetController = target.GetComponent<FlyingCharacterController2D>();

                if (target.layer == 9) // if hit player then initiate hit effects
                {
                    targetController.HitInflicted();
                }
                else
                {
                    if (targetController)
                        InitiateEnemyHitEffect(targetController);
                    if (flyingTargetController)
                        InitiateEnemyHitEffect(flyingTargetController);
                }
                Destroy(gameObject);
            }
        }
        
    }
    // TODO: 
    // Collapse both functions below into one
    // (they needed to be separate bc of different object types, "CharacterController2D" and "FlyingCharacterController2D")
    private void InitiateEnemyHitEffect(CharacterController2D targetController)
    {
        switch(element)
        {
            case "Snow":
                Debug.Log("Is snow");
                if (tier == 2)
                    targetController.SlowInflicted();
                break;
        }
    }

    private void InitiateEnemyHitEffect(FlyingCharacterController2D targetController)
    {
        switch (element)
        {
            case "Snow":
                Debug.Log("Is snow");
                if (tier == 2)
                    targetController.SlowInflicted();
                break;
        }
    }

    // Not used
    /*
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
    }*/

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
}
