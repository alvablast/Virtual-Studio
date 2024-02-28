using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// Benjamin Griffin
// using concepts from this tutorial https://www.youtube.com/watch?v=THnivyG0Mvo

public class FirstPersonCamera : MonoBehaviour
{
    public Camera mainCam;

    public Transform player;
    public Image crosshair; // normal crosshair
    public Image crosshairH; // highlight crosshair

    public float xSens = 700f;
    public float ySens = 700f;
    public float range;
    private float xRot;
    private float yRot;

    private RaycastHit _hit;

    private Transform _mcTransform;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _mcTransform = mainCam.transform;
        mainCam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySens;

        yRot += mouseX;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        mainCam.transform.rotation = Quaternion.Euler(xRot,yRot,0);
        transform.rotation = Quaternion.Euler(0, yRot, 0);

        if (Physics.Raycast(_mcTransform.position, _mcTransform.forward,out _hit, range))
        {
            Instrument instrument = _hit.transform.GetComponent<Instrument>();
            Debug.Log(_hit.transform.name);
            if (instrument != null)
            {
                crosshair.enabled = false;
                crosshairH.enabled = true;
                if (Input.GetMouseButtonDown(0))
                {
                    instrument.Activate();
                }
            }
        }
        else
        {
            crosshair.enabled = true;
            crosshairH.enabled = false;
        }
    }
}
