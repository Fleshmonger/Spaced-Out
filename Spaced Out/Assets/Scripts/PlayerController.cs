using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private bool charging = false, increasing = false;
    private float launchForce = 0f;

    public float minLaunchForce = 100f, maxLaunchForce = 200f, timeToMax = 1f;
    public Transform pitchTransform;
    public Text chargeText;

    private void Update()
    {
        if (charging)
        {
            Charge();
        }
        else
        {
            launchForce = 0;
        }
        chargeText.text = "" +  launchForce;
    }

    private void Charge()
    {
        float delta = (Time.deltaTime * (maxLaunchForce - minLaunchForce)) / timeToMax;
        if (increasing)
        {
            launchForce += delta;
            if (launchForce > maxLaunchForce)
            {
                launchForce = 2 * maxLaunchForce - launchForce;
                increasing = false;
            }
        }
        else
        {
            launchForce -= delta;
            if (launchForce < minLaunchForce)
            {
                launchForce = 2 * minLaunchForce - launchForce;
                increasing = true;
            }
        }
    }

    public void SetCharging(bool value)
    {
        charging = value;
        if (charging)
        {
            launchForce = minLaunchForce;
            increasing = true;
        }
        else
        {
            launchForce = 0f;
        }
    }

    public void Launch()
    {
        Launch(pitchTransform.forward);
    }

    public void Launch(Vector3 direction)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddForce(launchForce * direction.normalized);
    }
}