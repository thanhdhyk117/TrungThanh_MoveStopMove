using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] protected Animator animator;
    private string _currentAnimTrigger = "";

    [SerializeField] private List<Character> listCharacterTarget;
    [SerializeField] private ETargetType targetType = ETargetType.Nearest;
    [SerializeField] protected Character currentTarget => GetTarget();
    [SerializeField] private Character _previousTarget;

    [SerializeField] protected Weapon currentWeapon;

    [SerializeField] protected HideOnPlay hideOnPlay;

    private Action<Character> OnCharacterTrigger;

    [SerializeField] protected bool isAttacking = false;

    [SerializeField] protected bool isDead = false;

    public override void OnInit()
    {
        Debug.Log($"{name} initialized");
    }

    public override void OnDespawn()
    {

    }

    public void ChangeAnim(string anim)
    {
        if (anim != _currentAnimTrigger)
        {
            animator.ResetTrigger(_currentAnimTrigger);
            _currentAnimTrigger = anim;
            animator.SetTrigger(_currentAnimTrigger);
        }
    }

    public void AddCharacterTarget(Character character)
    {
        if (listCharacterTarget.Contains(character)) return;
        listCharacterTarget.Add(character);

        OnCharacterTrigger += RemoveCharacter;
    }

    public void RemoveCharacter(Character character)
    {
        if (!listCharacterTarget.Contains(character)) return;
        character.hideOnPlay?.ShowHideSymnol(false);
        listCharacterTarget.Remove(character);
        _previousTarget = null;
        OnCharacterTrigger -= RemoveCharacter;
    }

    private void OnDisable()
    {
        OnCharacterTrigger?.Invoke(this);
    }

    public virtual void OnDead()
    {
        Debug.Log($"{name} is dead");

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

        if (target != _previousTarget)
        {
            _previousTarget?.hideOnPlay?.ShowHideSymnol(false);
            target?.hideOnPlay?.ShowHideSymnol(true);
            _previousTarget = target;


        }

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


