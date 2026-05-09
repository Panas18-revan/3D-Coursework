using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;

    public float switchTime = 10f; // 8 minutes

    private float timer;
    private bool isNight = false;

    void Start()
    {
        SetDay();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchTime)
        {
            timer = 0;
            isNight = !isNight;

            if (isNight)
            {
                SetNight();
            }
            else
            {
                SetDay();
            }
        }
    }

    void SetNight()
    {
        // Make environment darker
        sun.intensity = 0.03f;

        // Night atmosphere color
        RenderSettings.ambientLight = new Color(0.07f, 0.07f, 0.12f);

        // Move sun angle lower
        sun.transform.rotation = Quaternion.Euler(340f, -30f, 0f);

        Debug.Log("Night Mode");
    }

    void SetDay()
    {
        // Bright daytime
        sun.intensity = 1f;

        // Day atmosphere
        RenderSettings.ambientLight = Color.white;

        // Normal sun angle
        sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        Debug.Log("Day Mode");
    }
}