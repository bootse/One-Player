using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class State<T>
{
    public List<Handler<T>> HandlerList = new List<Handler<T>>();
    virtual public void OnEnter(T owner)
    {

    }
    virtual public void Process(T owner)
    {

    }
    virtual public void OnExit(T owner)
    {

    }
    public void messageCalls(T owner, string call, params object[] arg)
    {
        foreach (Handler<T> handles in HandlerList)
        {
            handles.methodCall(call, owner, arg);
        }
    }
}

public class StateMachine<T>
{
    private T Owner;
    private State<T> CurrentState;
    private State<T> PreviousState;
    private State<T> GlobalState;
    private bool pause; //pause is to pause the current state, useful for menus
    private string STATE = "";

    public void Awake()
    {
        CurrentState = null;
        PreviousState = null;
        GlobalState = null;
    }

    public void Configure(T owner, State<T> InitialState)
    {
        Owner = owner;
        ChangeState(InitialState);
    }

    public void Update()
    {
        if (GlobalState != null) GlobalState.Process(Owner);
        if (CurrentState != null && !pause) CurrentState.Process(Owner);
    }

    public void ChangeGlobalState(State<T> globalState)
    {
        if (GlobalState != null)
            GlobalState.OnExit(Owner);
        GlobalState = globalState;
        if (GlobalState != null)
            GlobalState.OnEnter(Owner);
    }

    public void ChangeState(State<T> NewState)
    {
        PreviousState = CurrentState;
        if (CurrentState != null)
            CurrentState.OnExit(Owner);
        CurrentState = NewState;
        if (CurrentState != null)
            CurrentState.OnEnter(Owner);
    }

    public void RevertToPreviousState()
    {
        if (PreviousState != null)
            ChangeState(PreviousState);
    }

    public void messageReciever(string call, params object[] args)
    {
        messageReciever(call, false, args);
    }

    public void messageReciever(string call, bool globalState, params object[] args)
    {
        if (globalState)
        {
            GlobalState.messageCalls(Owner, call, args);
        }
        else
        {
            CurrentState.messageCalls(Owner, call, args);
        }
    }

    public bool isInCurrentState(State<T> state)
    {
        return CurrentState.Equals(state);
    }

    public void pauseCurrentState(bool p)
    {
        pause = p;
    }

    public bool isPaused()
    {
        return pause;
    }

    public string getState()
    {
        return STATE;
    }

    public void setState(string st)
    {
        STATE = st;
    }
}

public class Handler<T>
{
    private string receivedMessage;
    private Action<T, object[]> funcy;

    public Handler(string call, Action<T, object[]> methodName)
    {
        receivedMessage = call;
        funcy = methodName;
    }

    public void methodCall(string call, T owner, params object[] args)
    {
        if (call.Equals(receivedMessage))
        {
            funcy(owner, args);
        }
    }
}
