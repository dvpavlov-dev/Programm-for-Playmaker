using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class ButtonListner : MonoBehaviour
{
    public Button[] Buttons;

    // Start is called before the first frame update
    void Start()
    {
        Buttons = Button.FindObjectsOfType<Button>();

        var PMfsm = gameObject.AddComponent<PlayMakerFSM>();
        PMfsm.SendEvent("FINISHED");
        var states = PMfsm.FsmStates;
        PMfsm.Fsm.States = new FsmState[] { new FsmState(states[0]), new FsmState(states[0]), new FsmState(states[0]) };
        states = PMfsm.Fsm.States;

        UiButtonArray btnArray = new UiButtonArray();
        btnArray.gameObjects = new FsmGameObject[Buttons.Length];
        btnArray.clickEvents = new FsmEvent[Buttons.Length];
        for (int i = 0; i < Buttons.Length; i++)
        {
            btnArray.gameObjects[i] = Buttons[i].gameObject;
            btnArray.clickEvents[i] = FsmEvent.Finished;
        }

        FsmStateAction action1 = btnArray;

        Wait wait = new Wait();
        wait.time = 3;
        FsmStateAction action2 = wait;
        DebugLog debug = new DebugLog();
        debug.sendToUnityLog = true;
        debug.text = "Debug done";
        FsmStateAction action3 = debug;

        states[0].Actions = new FsmStateAction[] { action1 };
        states[0].Name = "State 1";
        states[0].SaveActions();

        states[1].Actions = new FsmStateAction[] { action2 };
        states[1].Name = "State 2";
        states[1].SaveActions();

        states[2].Actions = new FsmStateAction[] { action3 };
        states[2].Name = "State 3";
        states[2].SaveActions();

        FsmTransition Transition = new FsmTransition();
        FsmTransition Transition1 = new FsmTransition();
        FsmTransition Transition2 = new FsmTransition();

        states[0].Transitions = new FsmTransition[] { Transition };
        states[0].Transitions[0].FsmEvent = FsmEvent.Finished;
        states[0].Transitions[0].ToFsmState = states[1];

        states[1].Transitions = new FsmTransition[] { Transition1 };
        states[1].Transitions[0].FsmEvent = FsmEvent.Finished;
        states[1].Transitions[0].ToFsmState = states[2];

        states[2].Transitions = new FsmTransition[] { Transition2 };
        states[2].Transitions[0].FsmEvent = FsmEvent.Finished;
        states[2].Transitions[0].ToFsmState = states[0];

        PMfsm.Fsm.RestartOnEnable = true;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
