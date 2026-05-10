using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    public float speed = 5f;
    public float rotateSpeed = 5f;

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
        shouldStop = false;

        for (int i = 0; i < stopPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, stopPoints[i].position);

            if (distance < 5f)
            {
                if (trafficLights[i].currentState == TrafficLightController.LightState.Red ||
                    trafficLights[i].currentState == TrafficLightController.LightState.Yellow)
                {
                    shouldStop = true;
                    break;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Disable all player scripts
            foreach (var script in other.GetComponents<MonoBehaviour>())
            {
                script.enabled = false;
            }

            // 2. Disable Animator
            Animator anim = other.GetComponent<Animator>();
            if (anim != null) anim.enabled = false;

            // 3. Disable CharacterController
            var cc = other.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // 4. Add Rigidbody
            Rigidbody rb = other.gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.centerOfMass = new Vector3(0, 1.5f, 0); // high center of mass = tips over

            // 5. Launch player
            Vector3 force = transform.forward * 12f + Vector3.up * 7f;
            rb.AddForce(force, ForceMode.Impulse);

            // 6. Spin/tumble forward so it lands face down
            rb.AddTorque(Vector3.right * 5f, ForceMode.Impulse);

            // 7. Show UI
            if (hitMessageUI != null)
            {
                hitMessageUI.gameObject.SetActive(true);
                hitMessageUI.text = "You Died!";
            }

            // 8. Freeze after delay
            StartCoroutine(FreezeAfterHit());
        }
    }

    IEnumerator FreezeAfterHit()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 0f;
    }
}