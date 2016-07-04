using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Linq;

/*abstract public class State<T> {

    public State(T state)
    {
        owner = state;
    }

    public T owner;

    bool canSwitchStates;

    abstract public void UpdateState();

    abstract public void EnterState();

    abstract public void LeaveState();

    abstract public void AfterUpdate();

    public void SwitchState(State<T> newState)
    {
        Type t = owner.GetType();
        if (t != null)
        {
            MethodInfo mi = t.GetMethod("");
            if (mi != null)
            {
                mi.Invoke(this, new object[] { arguments });
            }
        }
    }

    // args is ',' delimeted string
    public void DoAction(string args)
    {
        string[] arguments = args.Split(',');
        string command = arguments[0];
        arguments = arguments.Skip(1).ToArray();
        Type t = GetType();
        if(t != null)
        {
            MethodInfo mi = t.GetMethod(command);
            if(mi != null)
            {
                mi.Invoke(this, new object[] { arguments });
            }
        }
        
    }

    public bool canLeave()
    {
        return canSwitchStates;
    }
}*/
