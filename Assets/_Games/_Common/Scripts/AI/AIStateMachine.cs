using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//https://drive.google.com/file/d/1fuvh8RnjXYVRxnlPioN7jsOepqCSf3T-/view
public class AIState
{
    public Action ActiveAction, OnEnterAction, OnExitAction;

    public AIState(Action active, Action onEnter, Action onExit)
    {
        ActiveAction = active;
        OnEnterAction = onEnter;
        OnExitAction = onExit;
    }

    public void Execute() => ActiveAction?.Invoke();
    public void OnEnter() => OnEnterAction?.Invoke();
    public void OnExit() => OnExitAction?.Invoke();
}

public class AIStateMachine : MonoBehaviour
{
    public Stack<AIState> states { get; set; } = new Stack<AIState>();

    private AIState currentState => states.Count > 0 ? states.Peek() : null;

    private void Update()
    {
        currentState?.Execute();
    }

    public void PushState(Action active, Action onEnter, Action onExit)
    {
        currentState?.OnExit();
        states.Push(new AIState(active, onEnter, onExit));
        currentState?.OnEnter();
    }

    public void PopState()
    {
        currentState?.OnExit();
        if (currentState != null) currentState.ActiveAction = null;
        states.Pop();
        currentState?.OnEnter();
    }
}
