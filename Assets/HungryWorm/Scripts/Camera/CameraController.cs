using System;
using HungryWorm;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_TargetedObject;
    [SerializeField] private float m_SmoothTime = 0.1f;
    
    [Header("Margins")]
    [SerializeField] private float depth = -10;
    [SerializeField] private float m_HorizontalMargin = 0.4f;
    [SerializeField] private float m_VerticalMargin = 0.4f;
    [SerializeField] private float m_AirTopMargin = 0.5f;
    [SerializeField] private float m_GroundedBottomMargin = 0.2f;
    [SerializeField] private float m_DeepGroundedBottomMargin = 0.4f;
    
    
    [Header("World Slices settings")]
    [SerializeField] private float m_bufferSize = 5f;
    
    
    private float cameraSemiWidth;
    
    private Vector3 currentVelocity;
    
    private Camera m_Camera;
    
    
    private Vector3 target;
    private Vector3 targetedObjectLastPosition;
    
    private float leftEnd;
    private float rightEnd;

    private float leftEdge;
    private float rightEdge;
    
    private SequenceManager sequenceManager;
    private string gamePlayStateName;

    private void Start()
    {
        sequenceManager = SequenceManager.Instance;
        gamePlayStateName = SequenceManager.GamePlayStateName;
        
        m_Camera = Camera.main;
        ResetCameraPosition();
        
        Vector3 leftEdgeVector = m_Camera.ViewportToWorldPoint(new Vector3(0, 0.5f, m_Camera.nearClipPlane));
        cameraSemiWidth = m_Camera.transform.position.x - leftEdgeVector.x;
    }

    private void OnEnable()
    {
        NullRefChecker.Validate(this);
        WorldEvents.LeftEdgeUpdated += WorldEvents_LeftEdgeUpdated;
        WorldEvents.RightEdgeUpdated += WorldEvents_RightEdgeUpdated;
    }
    
    private void OnDisable()
    {
        WorldEvents.LeftEdgeUpdated -= WorldEvents_LeftEdgeUpdated;
        WorldEvents.RightEdgeUpdated -= WorldEvents_RightEdgeUpdated;
    }

    private void Update()
    {
        if (sequenceManager.CurrentState.name != gamePlayStateName) return;
        
        
    }

    private void LateUpdate()
    {
        if (sequenceManager.CurrentState.name != gamePlayStateName) return;
        SetTarget();
        MoveCamera();
        CheckEdges();
    }

    private void CheckEdges()
    {
        leftEdge = Camera.main.transform.position.x - cameraSemiWidth;
        rightEdge = Camera.main.transform.position.x + cameraSemiWidth;

        if (leftEdge < leftEnd + m_bufferSize)
        {
            WorldEvents.MoveSlice?.Invoke(Direction.Left);
        }
        else if (rightEdge > rightEnd - m_bufferSize)
        {
            WorldEvents.MoveSlice?.Invoke(Direction.Right);
        }
    }
    
    private void SetTarget()
    {
        if (m_TargetedObject == null)
        {
            return;
        }
        
        Vector3 movementDelta = m_TargetedObject.position - targetedObjectLastPosition;
        Vector3 screenPos = m_Camera.WorldToScreenPoint(m_TargetedObject.position);

        bool isGrounded = m_TargetedObject.transform.position.y < -0.5f;

        float topMargin = 0;
        float bottomMargin = 0;

        float speed = 1;

        if (isGrounded)
        {
            topMargin = m_VerticalMargin;
            bottomMargin = m_GroundedBottomMargin;
            if (m_TargetedObject.transform.position.y < -5f)
            {
                speed = 1.2f;
                bottomMargin = m_DeepGroundedBottomMargin;
            }
        }
        else
        {
            topMargin = m_AirTopMargin;
            bottomMargin = m_VerticalMargin;
        }
        
        Vector3 bottomLeft = m_Camera.ViewportToScreenPoint(new Vector3(m_HorizontalMargin,bottomMargin,0));
        Vector3 topRight = m_Camera.ViewportToScreenPoint(new Vector3(1-m_HorizontalMargin, 1-topMargin, 0));

        if (screenPos.x < bottomLeft.x || screenPos.x > topRight.x)
        {
            target.x += movementDelta.x;
        }

        if (screenPos.y < bottomLeft.y || screenPos.y > topRight.y)
        {
            target.y += movementDelta.y * speed;
        }
        
        // Debug section :
        // Draw line to outline the margin area
        Debug.DrawLine(m_Camera.ScreenToWorldPoint(new Vector3(bottomLeft.x, bottomLeft.y, 10)), m_Camera.ScreenToWorldPoint(new Vector3(topRight.x, bottomLeft.y, 10)), Color.red);
        Debug.DrawLine(m_Camera.ScreenToWorldPoint(new Vector3(topRight.x, bottomLeft.y, 10)), m_Camera.ScreenToWorldPoint(new Vector3(topRight.x, topRight.y, 10)), Color.red);
        Debug.DrawLine(m_Camera.ScreenToWorldPoint(new Vector3(topRight.x, topRight.y, 10)), m_Camera.ScreenToWorldPoint(new Vector3(bottomLeft.x, topRight.y, 10)), Color.red);
        Debug.DrawLine(m_Camera.ScreenToWorldPoint(new Vector3(bottomLeft.x, topRight.y, 10)), m_Camera.ScreenToWorldPoint(new Vector3(bottomLeft.x, bottomLeft.y, 10)), Color.red);

        target.z = depth;
        targetedObjectLastPosition = m_TargetedObject.position;
    }

    private void WorldEvents_LeftEdgeUpdated(float leftEnd)
    {
        this.leftEnd = leftEnd;
    }
    
    private void WorldEvents_RightEdgeUpdated(float rightEnd)
    {
        this.rightEnd = rightEnd;
    }
    
    void MoveCamera()
    {
        //Debug.Log("Moving camera to " + target);
        m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, target, ref currentVelocity, m_SmoothTime);
        // m_Camera.transform.position = target;
    }

    private void ResetCameraPosition()
    {
        m_Camera.transform.position = new Vector3(0, 0, -10);
    }
}
