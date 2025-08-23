using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyCoin : GameUnit
{
    [SerializeField] RectTransform rect;
    [SerializeField] float moveSpeed;
    private Vector3 target;
    private UnityAction doneAction;
    float time;

    public void OnInit(Vector3 start, Vector3 target, UnityAction unityAction)
    {
        rect.position = start;
        this.target = target;
        this.doneAction = unityAction;
        time = 0;
    }

    private void LateUpdate()
    {
        time += Time.deltaTime;

        if (time > 1f) 
        {
            rect.position = Vector3.MoveTowards(rect.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(rect.position, target) < 10)
            {
                SimplePool.Despawn(this);
                doneAction?.Invoke();
            }
        }
    }

}
