using UnityEngine;

public class Laser : MonoBehaviour
{
    
    public void SetLaser(Vector2 start, Vector2 end)
    {
        transform.position = start;
        Vector2 direction = end - start;
        float distance = direction.magnitude;
        transform.right = direction.normalized;
        transform.localScale = new Vector3(distance, transform.localScale.y, 1);
    }
}
