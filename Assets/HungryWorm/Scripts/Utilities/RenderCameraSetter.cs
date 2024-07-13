using System;
using UnityEngine;

/// <summary>
/// Set the main camera as render camera for the canva
/// </summary>
public class RenderCameraSetter : MonoBehaviour
{
    private void OnEnable()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
}
