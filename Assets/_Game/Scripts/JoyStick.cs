using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStick : MonoBehaviour
{
    public static Vector3 direction;
    [SerializeField] private GameObject joystick;
    [SerializeField] private RectTransform bg, knob;
    [SerializeField] private float knobRange;
    private Vector3 startPos, currentPos;
    private Vector3 screen;
    private Vector3 MousePosition => Input.mousePosition - screen / 2;

    private void Awake()
    {
        screen.x = Screen.width;
        screen.y = Screen.height;
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = MousePosition;
            joystick.SetActive(true);
            bg.anchoredPosition = startPos;
        }

        if (Input.GetMouseButton(0))
        {
            currentPos = MousePosition;
            //calculate position of knob
            knob.anchoredPosition = Vector3.ClampMagnitude((currentPos - startPos), knobRange) + startPos;

            direction = (currentPos - startPos).normalized;
            direction.z = direction.y;
            direction.y = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            joystick.SetActive(false);
            direction = Vector3.zero;
            //direction.y = 0;
        }
    }


}
