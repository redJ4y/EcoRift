using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour
{
    public Vector2 ShadowOffset;
    public Material ShadowMaterial;

    SpriteRenderer spriteRenderer;
    GameObject shadowGameobject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //create a new gameobject to be used as drop shadow
        shadowGameobject = new GameObject("Shadow");
        shadowGameobject.transform.SetParent(transform);
        shadowGameobject.transform.localPosition = (Vector3)ShadowOffset;
        shadowGameobject.transform.localScale = transform.localScale;

        //create a new SpriteRenderer for Shadow gameobject
        SpriteRenderer shadowSpriteRenderer = shadowGameobject.AddComponent<SpriteRenderer>();
        shadowSpriteRenderer.flipX = spriteRenderer.flipX;

        //set the shadow gameobject's sprite to the original sprite
        shadowSpriteRenderer.sprite = spriteRenderer.sprite;
        //set the shadow gameobject's material to the shadow material we created
        shadowSpriteRenderer.material = ShadowMaterial;

        //update the sorting layer of the shadow to always lie behind the sprite
        shadowSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    void LateUpdate()
    {
        //update the position and rotation of the sprite's shadow with moving sprite
        /*
        shadowGameobject.transform.localPosition = transform.localPosition + (Vector3)ShadowOffset;
        shadowGameobject.transform.localRotation = transform.localRotation;
        shadowGameobject.transform.localScale = transform.localScale;
        */

        
    }
}