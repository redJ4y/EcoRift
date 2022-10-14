using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimingJoyStick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    [SerializeReference] private ProjectileHandler handler;
    [SerializeField]
    private float dragThreshold = 0.0001f;
    [SerializeField]
    private int dragMovementDistance = 30;
    [SerializeField]
    private int dragOffsetDistance = 100;

    public Vector2 aimVector;
    private RectTransform joystickTransform;
    private Coroutine shootRoutine;
    private float shootDelay;

    // Start is called before the first frame update
    void Start()
    {
        joystickTransform = (RectTransform)transform;
        handler = GameObject.Find("Player").GetComponent<ProjectileHandler>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTransform, eventData.position, null, out offset);
        offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;
        joystickTransform.anchoredPosition = offset * dragMovementDistance;

        aimVector = CalculateAim(offset);
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            handler.OnShoot();
            yield return new WaitForSeconds(shootDelay);
        }
    }

    private Vector2 CalculateAim(Vector2 offset)
    {
        float x = Mathf.Abs(offset.x) > dragThreshold ? offset.x : 0;
        float y = Mathf.Abs(offset.y) > dragThreshold ? offset.y : 0;
        return new Vector2(x, y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        shootDelay = 10.0f / handler.GetWeapon().GetComponent<Projectile>().GetFireRate();
        shootRoutine = StartCoroutine(ShootRoutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(shootRoutine);
        joystickTransform.anchoredPosition = Vector2.zero;
    }
}
