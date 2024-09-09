using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public List<Transform> waypoints; 
    public float speed = 20.0f; 
    public float brakeDistance = 10.0f; 

    private int currentWaypoint = 0;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = waypoints[0].position; 
    }

    void Update()
    {
        if (currentWaypoint < waypoints.Count - 1)
        {
            MoveToNextWaypoint();
        }
    }
    void MoveToNextWaypoint()
    {
        Vector3 target = waypoints[currentWaypoint + 1].position;
        Vector3 moveDirection = (target - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target);

        if (currentWaypoint == waypoints.Count - 2 && distance < brakeDistance)
        {
            float speedFactor = Mathf.Lerp(0, 1, distance / brakeDistance);
            rb.velocity = moveDirection * speed * speedFactor;
        }
        else
        {
            rb.velocity = moveDirection * speed; 
        }
        if (distance < 0.5f)
        {
            currentWaypoint++;
        }
    }
}
