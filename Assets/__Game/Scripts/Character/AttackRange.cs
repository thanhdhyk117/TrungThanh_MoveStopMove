using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField]private Character owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.TAG_CHARACTER))
        {
            var triggerCharacter = Cache.GetCharacter(other);
            
            if(triggerCharacter != owner)
                owner.AddCharacterTarget(other.GetComponent<Character>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Consts.TAG_CHARACTER))
        {
            var triggerCharacter = Cache.GetCharacter(other);
            if (triggerCharacter != owner)
                owner.RemoveCharacter(other.GetComponent<Character>());
        }
    }
        
}
