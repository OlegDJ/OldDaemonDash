using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    [SerializeField] private State curState;

    private void Start()
    {
        curState = GetInitialState();
        if (curState != null) curState.OnStateEnter();
    }

    protected virtual State GetInitialState() { return null; }

    private void Update() { if (curState != null) curState.StateUpdate(); }

    public void ChangeState(State newState)
    {
        curState.OnStateExit();

        curState = newState;
        curState.OnStateEnter();
    }

    private void OnGUI()
    {
        string content = curState != null ? curState.name : "No Current State";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    }
}
