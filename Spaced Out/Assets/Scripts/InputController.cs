using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
    private bool overTheShoulder = true;
    private Camera current;
    private Vector3[] cameraPositions;

    public bool tilt = false;
    public float cameraRotateSpeed = 120f;
    public Camera behind, above;
    public Transform charTransform, charPitchTransform;

    private Vector2 ScreenCenter()
    {
        return new Vector2(current.pixelWidth / 2f, current.pixelHeight / 2f);
    }

    private void Awake()
    {
        current = behind;
    }

    private void Update()
    {
        // Quit
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Look
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            Vector2 offset = pos - ScreenCenter();
            DirectedRotation(offset, tilt);
        }
        /*
        // Move
        if (Input.GetMouseButtonDown(1) && body.velocity.magnitude == 0f)
        {
            Vector3 direction = pitch.transform.forward;
            body.velocity = direction.normalized * launchSpeed;
        }
        */
    }

    public void DirectedRotation(Vector2 offset, bool tilt)
    {
        if (tilt)
        {
            charTransform.Rotate(Vector3.up, Time.deltaTime * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
            charPitchTransform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
        }
        else
        {
            Vector3 axis = new Vector3(-offset.y, offset.x, 0);
            charTransform.Rotate(axis, Time.deltaTime * cameraRotateSpeed * (offset.magnitude / ScreenCenter().magnitude));
        }
    }
}