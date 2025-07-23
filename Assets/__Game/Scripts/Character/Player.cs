using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private Transform skin;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            transform.Translate(direction * speed * Time.deltaTime);
            
            if (direction.magnitude > 0.1f)
            {
                skin.forward = direction;
                ChangeAnim(Consts.ANIM_RUN);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ChangeAnim(Consts.ANIM_IDLE);
        }
    }
}
