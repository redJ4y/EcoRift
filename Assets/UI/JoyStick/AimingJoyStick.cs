using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimingJoyStick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    private RectTransform joystickTransform;

    [SerializeReference] private ProjectileHandler handler;
    [SerializeField]
    private float dragThreshold = 0.0001f;
    [SerializeField]
    private int dragMovementDistance = 30;
    [SerializeField]
    private int dragOffsetDistance = 100;
    public Vector2 aimVector;
    //public event Action<Vector2> OnMove;
    public bool isShooting = false;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTransform, eventData.position, null, out offset);
        offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;
        joystickTransform.anchoredPosition = offset * dragMovementDistance;

        Vector2 inputVector = CalculateAim(offset);
       //OnMove?.Invoke(inputVector);
        isShooting = true;
    }


    private IEnumerator ShootRoutine()
    {
        while(true)
        {
            if(isShooting)
            {
                handler.OnShoot();
            }
                yield return new WaitForSeconds(0.1f);
        }
    }

    private Vector2 CalculateAim(Vector2 offset)
    {
        aimVector = new Vector2(offset.x, offset.y);
        float x = Mathf.Abs(offset.x) > dragThreshold ? offset.x : 0;
        float y = Mathf.Abs(offset.y) > dragThreshold ? offset.y : 0;
        return new Vector2(x, y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickTransform.anchoredPosition = Vector2.zero;
        //OnMove?.Invoke(Vector2.zero);
        isShooting = false;
    }

    private void Awake()
    {
        joystickTransform = (RectTransform)transform;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        handler = GameObject.Find("Player").GetComponent<ProjectileHandler>();
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
