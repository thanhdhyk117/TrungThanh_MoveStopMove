using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] private Animator animator;
    private int currentAnimTriggerIndex = -1;
    
    [SerializeField] private List<Character> listCharacterTarget;
    
    private Action<Character> OnCharacterDeath;
    
    public override void OnInit()
    {
        
    }

    public override void OnDespawn()
    {
        
    }

    public void ChangeAnim(int anim)
    {
        if (anim != currentAnimTriggerIndex)
        {
            animator.ResetTrigger(currentAnimTriggerIndex);
            currentAnimTriggerIndex = anim;
            animator.SetTrigger(currentAnimTriggerIndex);
        }
    }

    public void AddCharacterTarget(Character character)
    {
        if (listCharacterTarget.Contains(character)) return;
        listCharacterTarget.Add(character);

        OnCharacterDeath += RemoveCharacter;
    }
    
    public void RemoveCharacter(Character character)
    {
        if (!listCharacterTarget.Contains(character)) return;
        listCharacterTarget.Remove(character);
        OnCharacterDeath -= RemoveCharacter;
    }

    private void OnDisable()
    {
        OnCharacterDeath?.Invoke(this);
    }
}
