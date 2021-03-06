﻿using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
    private bool primed = false;
    private Vector2 oldPoint;
    private Camera current;
    private Plane inputPlane;

    public bool tilt = false, sidescroll = false, swipeRotate = true;
    public float cameraRotateSpeed = 4000f;
    public Camera behind, side;
    public BehindCamera behindCamera;
    public PlayerController player;
    public Transform playerTransform, playerPitchTransform;

    private void Awake()
    {
        inputPlane = new Plane(Vector3.right, Vector3.zero);
    }

    private void ResetRotation()
    {
        playerTransform.rotation = Quaternion.identity;
        playerPitchTransform.rotation = Quaternion.identity;
    }

    private Vector2 ScreenCenter()
    {
        return new Vector2(current.pixelWidth / 2f, current.pixelHeight / 2f);
    }

    private void Update()
    {
        // Quit
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (sidescroll)
        {
            current = side;
            side.enabled = true;
            behind.enabled = false;
 
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = current.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && player.gameObject.Equals(hit.collider.gameObject))
                {
                    primed = true;
                }
            }

            if (primed)
            {
                Vector2 center = ScreenCenter(), mouse = Input.mousePosition;
                Quaternion rotation = new Quaternion();
                rotation.SetLookRotation(-new Vector3(0f, mouse.y - center.y, center.x - mouse.x));
                playerPitchTransform.rotation = rotation;
                playerTransform.rotation = Quaternion.identity;
                player.GetComponent<Rigidbody>().freezeRotation = true;

                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = current.ScreenPointToRay(Input.mousePosition);
                    float enter;
                    if (inputPlane.Raycast(ray, out enter))
                    {
                        Vector3 distance = playerTransform.position - ray.GetPoint(enter);
                        player.LaunchScale(distance);
                    }
                    primed = false;
                    player.GetComponent<Rigidbody>().freezeRotation = false;
                }
            }
        }
        else
        {
            current = behind;
            behind.enabled = true;
            side.enabled = false;

            if (Input.GetMouseButtonDown(0))
            {
                oldPoint = Input.mousePosition;
            }

            // Look
            if (Input.GetMouseButton(0))
            {
                Vector2 pos = Input.mousePosition;
                Vector2 offset;
                if (swipeRotate)
                {
                    offset = pos - oldPoint;
                }
                else
                {
                    offset = pos - ScreenCenter();
                }
                DirectedRotation(offset, tilt);
                oldPoint = pos;
            }

            if (Input.GetMouseButton(1))
            {
                player.GetComponent<Rigidbody>().freezeRotation = true;
                playerTransform.rotation = behindCamera.transform.rotation;
                playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;
            }

            // Move
            if (Input.GetMouseButtonDown(1))
            {
                player.SetCharging(true);
            }

            // Launch
            if (Input.GetMouseButtonUp(1))
            {
                player.GetComponent<Rigidbody>().freezeRotation = false;
                player.LaunchCharge(behindCamera.pitch.transform.forward);
                player.SetCharging(false);
            }
        }
    }

    public void DirectedRotation(Vector2 offset, bool tilt)
    {
        /*
        if (tilt)
        {
            Vector3 axis = new Vector3(-offset.y, offset.x, 0);
            playerTransform.Rotate(axis, Time.deltaTime * cameraRotateSpeed * (offset.magnitude / ScreenCenter().magnitude));
        }
        else
        {
            float xScale = playerPitchTransform.up.y;
            playerTransform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
            playerPitchTransform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
        }
        */
        if (tilt)
        {
            Vector3 axis = new Vector3(-offset.y, offset.x, 0);
            behindCamera.transform.Rotate(axis, Time.deltaTime * cameraRotateSpeed * (offset.magnitude / ScreenCenter().magnitude));
        }
        else
        {
            float xScale = behindCamera.pitch.transform.up.y;
            behindCamera.transform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
            behindCamera.pitch.transform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
        }
    }
}