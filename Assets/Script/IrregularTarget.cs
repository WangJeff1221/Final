using UnityEngine;
public class IrregularTarget : MonoBehaviour
{
    public int score = 50;
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    private Vector3 moveDirection;
    private float timer = 0f;

    void Start()
    {
        moveDirection = Random.onUnitSphere;
        moveDirection.y = 0f;
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= changeDirectionInterval)
        {
            timer = 0f;
            moveDirection = Random.onUnitSphere;
            moveDirection.y = 0f; 
            moveDirection.Normalize();
        }

        if (Mathf.Abs(transform.position.x) > 20f || Mathf.Abs(transform.position.z) > 20f)
        {
            moveDirection = -moveDirection;
        }
    }
}
