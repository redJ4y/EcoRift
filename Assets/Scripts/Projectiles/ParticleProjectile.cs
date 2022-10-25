using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// currently this whole script relies on the assumption that only the player is shooting particles

public class ParticleProjectile : MonoBehaviour
{
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;
    public float particleDamage;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject target)
    {
        int numCollisionEvents = part.GetCollisionEvents(target, collisionEvents);

        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

        for (int i = 0; i < numCollisionEvents; i++)
        {
            if (rb)
            {
                target.transform.Find("HealthBar").GetComponent<Health>().TakeDamage(particleDamage);
            }
        }
    }
}
