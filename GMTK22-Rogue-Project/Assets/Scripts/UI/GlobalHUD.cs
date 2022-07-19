using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class GlobalHUD : MonoBehaviour
{
    [SerializeField]
    Button roolDiceButton;

    public void ToggleRoolDiceButton(bool state, Action<SessionStates> callBack = null, SessionStates stateToGo = SessionStates.PreDiceRoll)
    {
        void InvokeCallBack()
        {
            callBack?.Invoke(stateToGo);
        }

        if (state)
        {
            UnityAction buttonCallBack = new UnityAction(InvokeCallBack);

            roolDiceButton.gameObject.SetActive(true);
            roolDiceButton.onClick.AddListener(buttonCallBack);
        }
        else
        {
            roolDiceButton.onClick.RemoveAllListeners();
            roolDiceButton.gameObject.SetActive(false);
        }
    }

    public void ToggleDiceSlot(bool state)
    {

    }
}
