using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float moveThreshold = 0.1f;//how far you have to go before the camera has to move
    public float cameraMoveSpeed = 1.5f;//the max speed the camera can move
    public float maxDistance = 5.0f;//the max distance the camera can separate the reticle and the player
    public float moveCameraMargins = 0.1f;//what percent of the edge of the camera allows it to move focus from the player

    public GameObject player;//the player GameObject

    private Camera cam;
    private Vector2 screenMarginLower, screenMarginUpper;//the bounds where the camera can move away from the player
    private Vector2 prevScreenSize;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
        updateScreenSize();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }
        //Move to Player (or target position)
        Vector3 target = player.transform.position;
        Vector3 pointer = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.mousePosition.x < screenMarginLower.x || Input.mousePosition.x > screenMarginUpper.x
            || Input.mousePosition.y < screenMarginLower.y || Input.mousePosition.y > screenMarginUpper.y
            )
        {
            target = player.transform.position + ((pointer - player.transform.position) / 2);
            if ((target - player.transform.position).sqrMagnitude > maxDistance * maxDistance)
            {
                target = (target - player.transform.position).normalized * maxDistance + player.transform.position;
            }
            Debug.DrawLine(player.transform.position, target);
        }
        target.z = transform.position.z;
        if ((transform.position - target).sqrMagnitude >= moveThreshold * moveThreshold)
        {
            //2017-10-21: copied from Stonicorn.CameraController.LateUpdate()
            transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    (Vector3.Distance(
                        transform.position,
                        target) * cameraMoveSpeed + player.GetComponent<Rigidbody2D>().velocity.magnitude)
                        * Time.deltaTime);
        }

        //Check screen size
        if (prevScreenSize.x != cam.pixelWidth || prevScreenSize.y != cam.pixelHeight)
        {
            updateScreenSize();
        }

        //Check for exit
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void updateScreenSize()
    {
        screenMarginLower = new Vector2(cam.pixelWidth * moveCameraMargins, cam.pixelHeight * moveCameraMargins);
        screenMarginUpper = new Vector2(cam.pixelWidth - cam.pixelWidth * moveCameraMargins, cam.pixelHeight - cam.pixelHeight * moveCameraMargins);
        prevScreenSize.x = cam.pixelWidth;
        prevScreenSize.y = cam.pixelHeight;
    }
}
