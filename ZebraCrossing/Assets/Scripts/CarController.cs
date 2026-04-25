using UnityEngine;

public class CarController : MonoBehaviour
{
    public float speed = 5f;
    public Transform stopPoint;
    public TrafficLightController trafficLight;
    public GameObject hitMessageUI;

    public Transform[] waypoints; // 🔥 drag Waypoint1 and Waypoint2 here
    private int currentWaypoint = 0;

    private bool shouldStop = false;

    void Update()
    {
        CheckTrafficLight();

        if (!shouldStop)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];

        // Move toward waypoint
        transform.position = Vector3.MoveTowards(
            transform.position, target.position, speed * Time.deltaTime);

        // Rotate toward waypoint
        Vector3 dir = target.position - transform.position;
        if (dir.magnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRot, Time.deltaTime * 5f);
        }

        // Next waypoint when close enough
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitMessageUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}