
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;

    // Full day duration in seconds
    public float dayDuration = 480f; // 8 minutes

    void Update()
    {
        // Rotate sun slowly
        sun.transform.Rotate(Vector3.right * (360f / dayDuration) * Time.deltaTime);

        // Get current sun angle
        float angle = sun.transform.rotation.eulerAngles.x;


        // DAYTIME
        if (angle > 10 && angle < 170)
        {
            sun.intensity = 1f;

            RenderSettings.ambientLight = Color.white;
        }
        // NIGHTTIME
        else
        {
            sun.intensity = 0.05f;

            RenderSettings.ambientLight = new Color(0.07f, 0.07f, 0.12f);
        }
    }
}