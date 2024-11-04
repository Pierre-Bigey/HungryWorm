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
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        
        //Keep the y at the world 0 level
        
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
