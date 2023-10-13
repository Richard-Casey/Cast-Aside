using UnityEngine;

public class SwingLight : MonoBehaviour
{
    public float forceMagnitude = 0.1f; 
    public float forceInterval = 2.0f; 
    private float timer = 0.0f; 
    private Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = forceInterval; 
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= forceInterval)
        {
            
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);

            
            timer = 0.0f;
        }
    }
}