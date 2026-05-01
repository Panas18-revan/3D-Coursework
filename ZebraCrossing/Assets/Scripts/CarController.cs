using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 5f;
    public float rotateSpeed = 5f;
    public Transform stopPoint;
    public TrafficLightController trafficLight;
    public TMPro.TextMeshProUGUI hitMessageUI;

    public Transform[] waypoints;
    private int currentWaypoint = 0;

    private bool shouldStop = false;

    void Update()
    {
        CheckTrafficLight();

        if (!shouldStop)
        {
            FollowWaypoints();
        }
    }

    void FollowWaypoints()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];

        // Move toward waypoint
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Rotate toward waypoint
        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        // Check if reached waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0; // Loop back
        }
    }

    void CheckTrafficLight()
    {
        float distance = Vector3.Distance(transform.position, stopPoint.position);

        if (distance < 5f)
        {
            if (trafficLight.currentState == TrafficLightController.LightState.Red ||
                trafficLight.currentState == TrafficLightController.LightState.Yellow)
            {
                shouldStop = true;
            }
            else
            {
                shouldStop = false;
            }
        }
        else
        {
            shouldStop = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hitMessageUI != null)
            {
                hitMessageUI.gameObject.SetActive(true);
                hitMessageUI.text = "You Died!";
            }

            Time.timeScale = 0f;
        }
    }
}