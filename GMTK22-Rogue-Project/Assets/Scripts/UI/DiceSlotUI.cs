using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
public class DiceSlotUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI label;
    [SerializeField]
    Button button;
    [SerializeField]
    Transform sendBackPoint;

    [HorizontalLine]

    [SerializeField, ReadOnly]
    Dice storedDice;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Dices") && storedDice == null)
        {
            storedDice = other.gameObject.GetComponent<Dice>();
            storedDice.gameObject.SetActive(false);
            label.gameObject.SetActive(true);
            label.text = storedDice.value.ToString();
            button.gameObject.SetActive(true);
        }
    }

    public void ReleaseDice()
    {
        storedDice.transform.position = new Vector3(sendBackPoint.transform.position.x, storedDice.transform.position.y, sendBackPoint.transform.position.z);
        storedDice.gameObject.SetActive(true);
        storedDice = null;
        label.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
    }
}
