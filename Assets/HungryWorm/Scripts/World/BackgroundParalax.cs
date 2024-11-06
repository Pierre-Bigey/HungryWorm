using UnityEngine;


/// <summary>
/// This code comes from this video  : https://www.youtube.com/watch?v=UDtneE5Sygs
/// </summary>
public class BackgroundParallax : MonoBehaviour
{
    private float length, startpos;
    private GameObject cam;
    [SerializeField] private float parallaxEffect;
    
    void Start()
    {
        cam = Camera.main.gameObject;
        transform.parent.parent = cam.transform;
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        
        //Reset the world position y to 0
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
