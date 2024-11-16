using System;
using System.Collections;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private float lifetime;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        // Get the ParticleSystem component
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        // Play the particle system
        _particleSystem.Play();
        // Start the coroutine to wait for the lifetime of the particle system
        StartCoroutine(WaitAndDisable());
    }
    
    private IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(lifetime);
        // Debug.Log("Disable animation");
        gameObject.SetActive(false);
    }
}
