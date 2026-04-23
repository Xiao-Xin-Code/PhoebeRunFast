using UnityEngine;
using System;
    
public class TriggerArea : MonoBehaviour
{
    event Action OnEnter;
    event Action OnExit;

    public void RegisterEnterEvent(Action action)
    {
        OnEnter += action;
    }

    public void RegisterExitEvent(Action action)
    {
        OnExit += action;
    }

    public void UnRegisterEnterEvent(Action action)
    {
        OnEnter -= action;
    }

    public void UnRegisterExitEvent(Action action)
    {
        OnExit -= action;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnExit?.Invoke();
    }
}
