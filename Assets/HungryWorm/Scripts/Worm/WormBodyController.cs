using System;
using System.Collections.Generic;
using UnityEngine;

public class WormBodyController : MonoBehaviour
{
    [Header("HeadTarget")]
    [SerializeField] private Transform m_PlayerTarget;
    
    [Header("Body parts")]
    [SerializeField] private GameObject m_HeadPrefab;
    [SerializeField] private GameObject m_BodySegmentPrefab;
    [SerializeField] private GameObject m_TailPrefab;
    
    [Header("Body settings")]
    [SerializeField] private int m_BodySegmentsCount = 5;
    [SerializeField] private float m_DistanceFromHead = 0.5f;
    [SerializeField] private float m_DistanceBetweenSegments = 0.75f;
    
    private List<Transform> m_BodySegments;
    private List<Vector3> m_BodySegmentsPositionsLastFrame;
    
    
    private void Start()
    {
        m_BodySegments = new List<Transform>();
        m_BodySegmentsPositionsLastFrame = new List<Vector3>();
        
        CreateBody();
    }
    
    private void CreateBody()
    {
        GameObject head = Instantiate(m_HeadPrefab, Vector3.zero, Quaternion.identity, transform);
        Vector3 headPosition = m_PlayerTarget.position;
        head.transform.position = headPosition;
        m_BodySegments.Add(head.transform);
        m_BodySegmentsPositionsLastFrame.Add(head.transform.position);
        
        for (int i = 0; i < m_BodySegmentsCount; i++)
        {
            Vector3 position;
            if (i == 0)
            {
                position = m_PlayerTarget.position + Vector3.left * m_DistanceFromHead;
                position += Vector3.forward * 0.1f;
            }
            else
            {
                position = m_PlayerTarget.position +
                           Vector3.left * (m_DistanceBetweenSegments * i + m_DistanceFromHead);
                position += Vector3.forward * ( i + 1 ) * 0.1f;
            }
            GameObject bodySegment = Instantiate(m_BodySegmentPrefab, Vector3.zero, Quaternion.identity, transform);
            bodySegment.transform.position = position;
            m_BodySegments.Add(bodySegment.transform);
            m_BodySegmentsPositionsLastFrame.Add(bodySegment.transform.position);
        }
        
        Vector3 tailPosition = m_PlayerTarget.position + Vector3.left * (m_DistanceBetweenSegments * m_BodySegmentsCount + m_DistanceFromHead);
        tailPosition += Vector3.forward * ( m_BodySegmentsCount + 1 ) * 0.1f;
        GameObject tail = Instantiate(m_TailPrefab, Vector3.zero, Quaternion.identity, transform);
        tail.transform.position = tailPosition;
        m_BodySegments.Add(tail.transform);
        m_BodySegmentsPositionsLastFrame.Add(tail.transform.position);
    }

    private void LateUpdate()
    {
        MoveBody();
    }

    private void MoveBody()
    {
        //Update the head position and rotation
        m_BodySegments[0].position = m_PlayerTarget.position;
        m_BodySegments[0].rotation = m_PlayerTarget.rotation;
        
        //Compare the current transform of the body segments with the last frame transform
        //and add this difference to the current transform of the next body segment
        for (int i = 1; i < m_BodySegments.Count; i++)
        {
            Vector3 difference = m_BodySegments[i].position - m_BodySegments[i-1].position;
            //Check if the distance is greater than the distance between segments
            if (difference.magnitude > m_DistanceBetweenSegments)
            {
                //Move the body element to be at the same distance from the previous element
                Vector3 direction = difference.normalized;
                m_BodySegments[i].position -= direction * (difference.magnitude - m_DistanceBetweenSegments);
                
                float angle = Vector3.SignedAngle(Vector3.right, -difference, Vector3.forward);
                m_BodySegments[i].eulerAngles = angle * Vector3.forward;
            }
            
            
        }
        //Save the current transform of the body segments for the next frame
        for (int i = 0; i < m_BodySegments.Count; i++)
        {
            m_BodySegmentsPositionsLastFrame[i] = m_BodySegments[i].position;
        }
        
    }
}
