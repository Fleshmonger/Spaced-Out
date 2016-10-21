using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
    private Camera current;

    public bool tilt = false;
    public float cameraRotateSpeed = 120f;
    public Camera behind, above;
    public PlayerController player;
    public Transform playerTransform, playerPitchTransform;

    private void ResetRotation()
    {
        playerTransform.rotation = Quaternion.identity;
        playerPitchTransform.rotation = Quaternion.identity;
    }

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

        // Move
        if (Input.GetMouseButtonDown(1))
        {
            player.SetCharging(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            player.Launch();
            player.SetCharging(false);
        }
    }

    public void DirectedRotation(Vector2 offset, bool tilt)
    {
        if (tilt)
        {
            Vector3 axis = new Vector3(-offset.y, offset.x, 0);
            playerTransform.Rotate(axis, Time.deltaTime * cameraRotateSpeed * (offset.magnitude / ScreenCenter().magnitude));
        }
        else
        {
            playerTransform.Rotate(Vector3.up, Time.deltaTime * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
            playerPitchTransform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
        }
    }
}