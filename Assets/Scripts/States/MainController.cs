using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {

    public State<MainController>[] states = new State<MainController>[]
    {
        new Menu(),
        new Library(),
        new Player()
    };

    public StateMachine<MainController> stateMachine = new StateMachine<MainController>();

	// Use this for initialization
	void Start () {
        stateMachine.Configure(this, states[0]);
        stateMachine.ChangeGlobalState(states[3]);
    }

	// Update is called once per frame
	void Update () {
        stateMachine.Update();
    }
}