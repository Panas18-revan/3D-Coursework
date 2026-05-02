using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 5f;
    public float rotateSpeed = 5f;

    // Remove single stopPoint and trafficLight
    // Add arrays instead:
    public Transform[] stopPoints;
    public TrafficLightController[] trafficLights;

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

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
                currentWaypoint = 0;
        }
    }

    void CheckTrafficLight()
    {
        shouldStop = false; // Reset every frame

        for (int i = 0; i < stopPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, stopPoints[i].position);

            if (distance < 5f)
            {
                if (trafficLights[i].currentState == TrafficLightController.LightState.Red ||
                    trafficLights[i].currentState == TrafficLightController.LightState.Yellow)
                {
                    shouldStop = true;
                    break; // No need to check further
                }
            }
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