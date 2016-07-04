using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

abstract public class State<T>
{

    private T owner;
    /// <summary>
    /// Call upward to get the base GameObject or other necessary parent properties or functions
    /// </summary>
    public T Owner
    {
        get { return owner; }
        private set { owner = value; }
    }

    [Tooltip("Is the state initialized")]
    private bool initialized;

    /// <summary>
    /// Setup for the state equivalent to constructor
    /// </summary>
    /// <param name="owner">setting parent variable</param>
    virtual public void Configure(T owner)
    {
        Owner = owner;
        initialized = true;
    }

    /// <summary>
    /// Startup for the state to run smooth.
    /// Assign any used null variables here.
    /// </summary>
    abstract public void OnEnter();
    
    /// <summary>
    /// Actions and checks that happen every frame.
    /// </summary>
    abstract public void Process();

    /// <summary>
    /// Cleanup to safely transition states.
    /// </summary>
    abstract public void OnExit();

    /// <summary>
    /// Used for the background process.
    /// </summary>
    abstract public void GlobalProcess();

    /// <summary>
    /// Through reflection, send the message to the first active state that can take the method (nested state machines).
    /// </summary>
    /// <param name="call">Function being called</param>
    /// <param name="args">arguments for the function to use</param>
    public void messageCalls(string call, params object[] args)
    {
        Type t = GetType();
        if(t != null)
        {
            MethodInfo mi = t.GetMethod(call);
            if (mi != null)
            {
                mi.Invoke(this, args);
            }
            else
            {
                StateMachine<T> sm = t.GetMember("stateMachine") as StateMachine<T>;
                if (sm != null)
                {
                    sm.messageReciever(call, args);
                }
            }
        }
    }
    
    public bool canRun()
    {
        return initialized;
    }
}

/// <summary>
/// NAME ALL VARIABLES "stateMachine" 
/// Value is used to find nested state machines.
/// Finite State Machines are used to run specific sections of code at a time.
/// </summary>
/// <typeparam name="T">The class that is running the state machine</typeparam>
public class StateMachine<T>
{
    private T owner;
    /// <summary>
    /// Call upward to get the base GameObject or other necessary parent properties or functions
    /// </summary>
    public T Owner
    {
        get { return owner; }
        private set { owner = value; }
    }

    [Tooltip("Active state running")]
    private State<T> currentState;

    [Tooltip("Previous state, shouldn't be used extensively")]
    private State<T> previousState;

    [Tooltip("Background state to run throughout the program.")]
    private State<T> globalState;

    private bool pause;
    /// <summary>
    /// pause the current state, useful for menus
    /// </summary>
    public bool Pause
    {
        get { return pause; }
        set { pause = value; }
    }

    private string state;
    /// <summary>
    /// String for the active state,
    /// Shouldn't be used extensively
    /// </summary>
    public string State
    {
        get { return state; }
        set { state = value; }
    }

    /// <summary>
    /// preventing "missing" components
    /// </summary>
    public void Awake()
    {
        currentState = null;
        previousState = null;
        globalState = null;
    }

    /// <summary>
    /// Initial setup for the state machine
    /// </summary>
    /// <param name="owner">reference to the parent class</param>
    /// <param name="InitialState">starting state that is used</param>
    public void Configure(T owner, State<T> InitialState)
    {
        this.owner = owner;
        ChangeState(InitialState);
    }

    /// <summary>
    /// Main actions in the state machine
    /// </summary>
    public void Update()
    {
        if (globalState != null) globalState.Process();
        if (currentState != null && !pause) currentState.Process();
    }

    /// <summary>
    /// Exits previous global state and enters a new one
    /// </summary>
    /// <param name="newGlobalState">new global state for the state machine</param>
    public void ChangeGlobalState(State<T> newGlobalState)
    {
        if (globalState != null)
            globalState.OnExit();
        globalState = newGlobalState;
        if (globalState != null)
        {
            if (!globalState.canRun())
            {
                globalState.Configure(owner);
            }
            globalState.OnEnter();
        }
            
    }

    /// <summary>
    /// Stores current state as previous, exits old state, enters new state
    /// </summary>
    /// <param name="NewState">new state that the state machine runs</param>
    public void ChangeState(State<T> NewState)
    {
        previousState = currentState;
        if (currentState != null)
            currentState.OnExit();
        currentState = NewState;
        if (currentState != null)
        {
            if(!currentState.canRun())
            {
                currentState.Configure(owner);
            }
            currentState.OnEnter();
        }
    }

    /// <summary>
    /// Changes state to the previous state
    /// rare use case
    /// </summary>
    public void RevertToPreviousState()
    {
        if (previousState != null)
            ChangeState(previousState);
    }

    /// <summary>
    /// calling internal state functions
    /// </summary>
    /// <param name="call">function that will be called</param>
    /// <param name="args">arguments for the function</param>
    public void messageReciever(string call, params object[] args)
    {
        messageReciever(call, false, args);
    }

    /// <summary>
    /// calling internal state functions
    /// </summary>
    /// <param name="call">function that will be called</param>
    /// <param name="globalState">sends to the global state</param>
    /// <param name="args">arguments for the function</param>
    public void messageReciever(string call, bool globalState, params object[] args)
    {
        if (globalState)
        {
            this.globalState.messageCalls(call, args);
        }
        else
        {
            currentState.messageCalls(call, args);
        }
    }

    public bool isInCurrentState(State<T> state)
    {
        return currentState.Equals(state);
    }
}
