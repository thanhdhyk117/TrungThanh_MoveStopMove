using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] protected Animator animator;
    private string currentAnimTrigger = "";

    [SerializeField] private List<Character> listCharacterTarget;
    [SerializeField] private ETargetType targetType = ETargetType.Nearest;
    [SerializeField] protected Character currentTarget => GetTarget();
    [SerializeField] protected HideOnPlay hideOnPlay;
    private Action<Character> OnCharacterDeath;

    [SerializeField] protected bool isAttacking = false;

    public override void OnInit()
    {

    }

    public override void OnDespawn()
    {

    }

    public void ChangeAnim(string anim)
    {
        if (anim != currentAnimTrigger)
        {
            animator.ResetTrigger(currentAnimTrigger);
            currentAnimTrigger = anim;
            animator.SetTrigger(currentAnimTrigger);
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
        character.hideOnPlay?.ShowHideSymnol(false);
        listCharacterTarget.Remove(character);
        OnCharacterDeath -= RemoveCharacter;
    }

    private void OnDisable()
    {
        OnCharacterDeath?.Invoke(this);
    }

    public Character GetTarget()
    {
        if (listCharacterTarget.Count == 0) return null;
        Character target = null;
        switch (targetType)
        {
            case ETargetType.Nearest:
                target = GetNearestTarget();
                break;
            case ETargetType.Farthest:
                target = GetFarthestTarget();
                break;
            default:
                break;
        }
        target.hideOnPlay?.ShowHideSymnol(true);
        return target;
    }

    private Character GetFarthestTarget()
    {
        if (listCharacterTarget.Count == 0) return null;

        Character farthestTarget = listCharacterTarget[0];
        float maxDistance = 0f;

        for (var i = 1; i < listCharacterTarget.Count; i++)
        {
            var distance = Vector3.Distance(transform.position, listCharacterTarget[i].transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestTarget = listCharacterTarget[i];
            }
        }

        return farthestTarget;
    }

    private Character GetNearestTarget()
    {
        if (listCharacterTarget.Count == 0) return null;

        Character nearestTarget = listCharacterTarget[0];
        float maxDistance = 0f;

        for (var i = 1; i < listCharacterTarget.Count; i++)
        {
            var distance = Vector3.Distance(transform.position, listCharacterTarget[i].transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                nearestTarget = listCharacterTarget[i];
            }
        }

        return nearestTarget;
    }
}


