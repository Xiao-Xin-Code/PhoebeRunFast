using System;
using UnityEngine;

public class CollisionArea : MonoBehaviour
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



    private void OnCollisionEnter(Collision collision)
    {
        OnEnter?.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        OnExit?.Invoke();
    }

}
