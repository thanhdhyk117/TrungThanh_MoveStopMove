using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : GameUnit
{
    [Header("Character Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Weapon currentWeapon;
    [SerializeField] protected HideOnPlay hideOnPlay;
    [SerializeField] protected Transform skin;

    [Header("Movement Settings")]
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float threshold = 0.1f;

    [Header("Attack Settings")]
    [SerializeField] protected float timeToDelay = 1f;
    [SerializeField] protected float attackCooldown = 1.5f;

    [Header("Targeting")]
    [SerializeField] private List<Character> listCharacterTarget = new List<Character>();
    [SerializeField] private ETargetType targetType = ETargetType.Nearest;
    [SerializeField] protected Character currentTarget => GetTarget();
    [SerializeField] private Character _previousTarget;

    // State management
    private string _currentAnimTrigger = "";
    private Action<Character> OnCharacterTrigger;
    protected bool isAttacking = false;
    protected bool isDead = false;
    protected bool _canAttack = true;
    
    // Attack timing
    protected TimeCounter _timeCounter;
    protected Coroutine _attackCooldownCoroutine;

    public override void OnInit()
    {
        Debug.Log($"{name} initialized");
        isDead = false;
        isAttacking = false;
        _canAttack = true;
        _previousTarget = null;
        _currentAnimTrigger = "";
        
        if (_timeCounter == null)
            _timeCounter = new TimeCounter();
    }

    public override void OnDespawn()
    {
        listCharacterTarget.Clear();
        _previousTarget = null;
        OnCharacterTrigger = null;
        _timeCounter?.Stop();
        
        if (_attackCooldownCoroutine != null)
        {
            StopCoroutine(_attackCooldownCoroutine);
            _attackCooldownCoroutine = null;
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        HandleCombat();
        _timeCounter?.Excute(Time.deltaTime);
    }

    #region Animation Management
    
    public void ChangeAnim(string anim)
    {
        if (isDead) return;
        
        if (anim != _currentAnimTrigger)
        {
            if (!string.IsNullOrEmpty(_currentAnimTrigger))
            {
                animator.ResetTrigger(_currentAnimTrigger);
            }
            _currentAnimTrigger = anim;
            animator.SetTrigger(_currentAnimTrigger);
        }
    }

    #endregion

    #region Movement

    protected virtual void Movement(Vector3 direction)
    {
        if (isDead) return;

        if (direction.magnitude > threshold)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            
            if (skin != null)
                skin.forward = direction;
                
            ChangeAnim(Consts.ANIM_RUN);
        }
        else
        {
            ChangeAnim(Consts.ANIM_IDLE);
        }
    }

    protected void LookAtTarget(Character target)
    {
        if (target == null || skin == null) return;
        
        Vector3 direction = (target.TF.position - TF.position).normalized;
        if (direction != Vector3.zero)
        {
            skin.forward = direction;
        }
    }

    #endregion

    #region Combat System

    protected virtual void HandleCombat()
    {
        if (currentTarget == null || !CanAttack()) return;

        LookAtTarget(currentTarget);
        PrepareAttack();
    }

    protected virtual bool CanAttack()
    {
        return _canAttack && 
               !isAttacking && 
               !isDead && 
               !_timeCounter.IsRunning;
    }

    protected virtual void PrepareAttack()
    {
        if (!CanAttack()) return;
        
        ChangeAnim(Consts.ANIM_ATTACK);
        _timeCounter.Run(ExecuteAttack, timeToDelay);
    }

    protected virtual void ExecuteAttack()
    {
        if (!_canAttack || currentTarget == null || isDead) return;

        _canAttack = false;
        isAttacking = true;

        // Fire weapon
        currentWeapon?.Fire();

        // Start cooldown
        _attackCooldownCoroutine = StartCoroutine(AttackCooldown());
    }

    protected virtual IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        
        _canAttack = true;
        isAttacking = false;
        _attackCooldownCoroutine = null;
        
        // Return to idle if no other actions
        OnAttackComplete();
    }

    protected virtual void OnAttackComplete()
    {
        if (!isDead)
        {
            ChangeAnim(Consts.ANIM_IDLE);
        }
    }

    #endregion

    #region Target Management

    public void AddCharacterTarget(Character character)
    {
        if (character == null || listCharacterTarget.Contains(character) || character.isDead) 
            return;
            
        listCharacterTarget.Add(character);
        character.OnCharacterTrigger += RemoveCharacter;
    }

    public void RemoveCharacter(Character character)
    {
        if (character == null || !listCharacterTarget.Contains(character)) 
            return;
            
        character.hideOnPlay?.ShowHideSymnol(false);
        listCharacterTarget.Remove(character);
        
        if (_previousTarget == character)
        {
            _previousTarget = null;
        }
        
        character.OnCharacterTrigger -= RemoveCharacter;
    }

    public Character GetTarget()
    {
        // Remove dead targets
        for (int i = listCharacterTarget.Count - 1; i >= 0; i--)
        {
            if (listCharacterTarget[i] == null || listCharacterTarget[i].isDead)
            {
                listCharacterTarget.RemoveAt(i);
            }
        }

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
                target = listCharacterTarget[0];
                break;
        }

        UpdateTargetHighlight(target);
        return target;
    }

    private void UpdateTargetHighlight(Character target)
    {
        if (target != _previousTarget)
        {
            _previousTarget?.hideOnPlay?.ShowHideSymnol(false);
            target?.hideOnPlay?.ShowHideSymnol(true);
            _previousTarget = target;
        }
    }

    private Character GetNearestTarget()
    {
        if (listCharacterTarget.Count == 0) return null;

        Character nearestTarget = listCharacterTarget[0];
        float minDistance = Vector3.Distance(transform.position, nearestTarget.transform.position);

        for (int i = 1; i < listCharacterTarget.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, listCharacterTarget[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = listCharacterTarget[i];
            }
        }

        return nearestTarget;
    }

    private Character GetFarthestTarget()
    {
        if (listCharacterTarget.Count == 0) return null;

        Character farthestTarget = listCharacterTarget[0];
        float maxDistance = Vector3.Distance(transform.position, farthestTarget.transform.position);

        for (int i = 1; i < listCharacterTarget.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, listCharacterTarget[i].transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestTarget = listCharacterTarget[i];
            }
        }

        return farthestTarget;
    }

    #endregion

    #region Death System

    public virtual void OnDead()
    {
        if (isDead) return;
        
        isDead = true;
        isAttacking = false;
        _canAttack = false;
        _timeCounter?.Stop();
        
        Debug.Log($"{name} is dead");
        hideOnPlay?.ShowHideSymnol(false);
        
        HandleDeath();
    }

    protected virtual void HandleDeath()
    {
        // Override in derived classes for specific death behavior
        ChangeAnim(Consts.ANIM_DEAD);
    }

    protected IEnumerator DeathSequence(float waitTime = 1f)
    {
        ChangeAnim(Consts.ANIM_DEAD);
        
        // Wait for death animation to complete
        yield return new WaitUntil(() => 
            animator.GetCurrentAnimatorStateInfo(0).IsName("Dead") && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        yield return new WaitForSeconds(waitTime);
        
        OnDespawn();
        SimplePool.Despawn(this);
    }

    #endregion

    private void OnDisable()
    {
        OnCharacterTrigger?.Invoke(this);
    }
}