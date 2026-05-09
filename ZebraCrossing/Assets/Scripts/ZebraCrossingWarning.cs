using UnityEngine;
using System.Collections;

public class ZebraCrossingWarning : MonoBehaviour
{
    public GameObject warningUI;
    public GameObject congratsUI;
    public TrafficLightController trafficLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (trafficLight.currentState == TrafficLightController.LightState.Green)
            {
                StartCoroutine(ShowUI(warningUI));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            warningUI.SetActive(false);
            if (trafficLight.currentState == TrafficLightController.LightState.Red)
            {
                StartCoroutine(ShowUI(congratsUI));
            }
        }
    }

    IEnumerator ShowUI(GameObject ui)
    {
        ui.SetActive(true);
        yield return new WaitForSeconds(3f);
        ui.SetActive(false);
    }
}