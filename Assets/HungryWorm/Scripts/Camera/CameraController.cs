using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_TargetedObject;
    [SerializeField] private float m_SmoothTime = 0.25f;
    
    [Header("Margins")]
    [SerializeField] private float depth = -10;
    [SerializeField] private float m_HorizontalMargin = 0.1f;
    [SerializeField] private float m_VerticalMargin = 0.1f;
    
    
    private Vector3 currentVelocity;
    
    private Camera m_Camera;
    
    
    private Vector3 target;
    private Vector3 targetedObjectLastPosition;

    private void Start()
    {
        m_Camera = Camera.main;
    }

    private void LateUpdate()
    {
        SetTarget();
        MoveCamera();
    }
    
    private void SetTarget()
    {
        if (m_TargetedObject == null)
        {
            return;
        }
        
        Vector3 movementDelta = m_TargetedObject.position - targetedObjectLastPosition;
        Vector3 screenPos = m_Camera.WorldToScreenPoint(m_TargetedObject.position);
        Vector3 bottomLeft = m_Camera.ViewportToScreenPoint(new Vector3(m_HorizontalMargin,m_VerticalMargin,0));
        Vector3 topRight = m_Camera.ViewportToScreenPoint(new Vector3(1-m_HorizontalMargin, 1-m_VerticalMargin, 0));
        
        //debug draw line to show margins
        Debug.DrawLine(m_Camera.ScreenToWorldPoint(new Vector3(bottomLeft.x, bottomLeft.y, depth)), m_Camera.ScreenToWorldPoint(new Vector3(topRight.x, bottomLeft.y, depth)), Color.red);
        Debug.DrawLine(m_Camera.ScreenToWorldPoint(new Vector3(topRight.x, bottomLeft.y, depth)), m_Camera.ScreenToWorldPoint(new Vector3(topRight.x, topRight.y, depth)), Color.red);
        Debug.DrawLine(m_Camera.ScreenToWorldPoint(new Vector3(topRight.x, topRight.y, depth)), m_Camera.ScreenToWorldPoint(new Vector3(bottomLeft.x, topRight.y, depth)), Color.red);
        Debug.DrawLine(m_Camera.ScreenToWorldPoint(new Vector3(bottomLeft.x, topRight.y, depth)), m_Camera.ScreenToWorldPoint(new Vector3(bottomLeft.x, bottomLeft.y, depth)), Color.red);
        

        if (screenPos.x < bottomLeft.x || screenPos.x > topRight.x)
        {
            target.x += movementDelta.x;
        }

        if (screenPos.y < bottomLeft.y || screenPos.y > topRight.y)
        {
            target.y += movementDelta.y;
        }

        target.z = depth;
        targetedObjectLastPosition = m_TargetedObject.position;
    }
    
    void MoveCamera()
    {
        m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, target, ref currentVelocity, m_SmoothTime);
    }
}
