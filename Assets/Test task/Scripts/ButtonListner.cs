using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListner : MonoBehaviour
{
    private Button[] _buttons;
    private FsmState[] _states;

    void Start()
    {
        _buttons = Button.FindObjectsOfType<Button>();

        var PMfsm = gameObject.AddComponent<PlayMakerFSM>();
        PMfsm.SendEvent("FINISHED");

        _states = CreateStates(PMfsm);

        UiButtonArray btnArray = new UiButtonArray();
        btnArray.gameObjects = new FsmGameObject[_buttons.Length];
        btnArray.clickEvents = new FsmEvent[_buttons.Length];
        for (int i = 0; i < _buttons.Length; i++)
        {
            btnArray.gameObjects[i] = _buttons[i].gameObject;
            btnArray.clickEvents[i] = FsmEvent.Finished;
        }
        btnArray.clickIndex = 0;
        FsmStateAction action1 = btnArray;

        Wait wait = new Wait();
        wait.time = 3;
        FsmStateAction action2 = wait;

        DebugLog debug = new DebugLog();
        debug.sendToUnityLog = true;
        debug.text = "Debug done";
        FsmStateAction action3 = debug;

        _states[0].Name = "State 1";
        CreateAction(0, action1);

        _states[1].Name = "Wait";
        CreateAction(1, action2);

        _states[2].Name = "Debug";
        CreateAction(2, action3);

        FsmTransition transition1 = new FsmTransition();
        FsmTransition transition2 = new FsmTransition();
        FsmTransition transition3 = new FsmTransition();

        CreateTransition(0, 0, transition1, FsmEvent.Finished, _states[1]);
        CreateTransition(1, 0, transition2, FsmEvent.Finished, _states[2]);
        CreateTransition(2, 0, transition3, FsmEvent.Finished, _states[0]);

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private FsmState[] CreateStates(PlayMakerFSM PMfsm)
    {
        var states = PMfsm.FsmStates;
        PMfsm.Fsm.States = new FsmState[] { new FsmState(states[0]), new FsmState(states[0]), new FsmState(states[0]) };
        return PMfsm.Fsm.States;
    }

    private void CreateAction(int stateIndex, FsmStateAction action)
    {
        _states[stateIndex].Actions = new FsmStateAction[] { action };
        _states[stateIndex].SaveActions();
    }

    private void CreateTransition(int stateIndex, int trasitionIndex, FsmTransition transition, FsmEvent clickEvent, FsmState transitionTo)
    {
        _states[stateIndex].Transitions = new FsmTransition[] { transition };
        _states[stateIndex].Transitions[trasitionIndex].FsmEvent = clickEvent;
        _states[stateIndex].Transitions[trasitionIndex].ToFsmState = transitionTo;
    }
}
