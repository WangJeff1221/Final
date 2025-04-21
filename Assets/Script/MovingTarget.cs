using UnityEngine;
public class MovingTarget : MonoBehaviour
{
    public int score = 20;
    public float moveSpeed = 3f;
    private Vector3 direction = Vector3.right; 

    void Update()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 15f) 
        {
            direction = -direction;
        }
    }
}
