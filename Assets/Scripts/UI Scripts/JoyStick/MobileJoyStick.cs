using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoyStick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    private RectTransform joystickTransform;
   
    [SerializeField]
    private float dragThreshold = 0.0001f;
    [SerializeField]
    private int dragMovementDistance = 30;
    [SerializeField]
    private int dragOffsetDistance = 100;
    public Vector2 aimVector;
    public event Action<Vector2> OnMove;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTransform, eventData.position, null, out offset);
        offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;
        joystickTransform.anchoredPosition = offset * dragMovementDistance;
        Vector2 inputVector = CalculateMovementInput(offset);
        OnMove?.Invoke(inputVector);
    }

    private Vector2 CalculateMovementInput(Vector2 offset)
    {
        aimVector = new Vector2(offset.x, offset.y);
        float x = Mathf.Abs(offset.normalized.x) > dragThreshold ? offset.x : 0;
        float y = Mathf.Abs(offset.normalized.y) > dragThreshold ? offset.y : 0;
        return new Vector2(x, y);
    }

    public void OnPointerDown(PointerEventData eventData) // unused interface method
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickTransform.anchoredPosition = Vector2.zero;
        OnMove?.Invoke(Vector2.zero);
    }

    private void Awake()
    {
        joystickTransform = (RectTransform)transform;
    }
}
