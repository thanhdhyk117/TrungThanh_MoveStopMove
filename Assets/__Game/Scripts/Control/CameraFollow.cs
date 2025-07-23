using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   [SerializeField] private Transform target;
   [SerializeField] private Vector3 offset;
   private void LateUpdate()
   {
      transform.position = Vector3.Lerp(transform.position, target.position + offset, 5 * Time.deltaTime);
   }
}
