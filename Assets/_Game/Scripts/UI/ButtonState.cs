using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonState : MonoBehaviour
{
    public enum State { Buy, Equip, Equiped }

    [SerializeField] GameObject[] buttonObjects;

    public void SetState(State state)
    {
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            buttonObjects[i].SetActive(false);
        }

        buttonObjects[(int)state].SetActive(true);
    }

}
