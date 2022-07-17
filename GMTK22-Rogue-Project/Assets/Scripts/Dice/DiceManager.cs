using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private int startDiceNbr;

    [SerializeField] private Transform diceBankTransform;
    [SerializeField] private List<Dice> diceBank;
    [SerializeField] private List<Dice> diceHand;

    [SerializeField] private Transform anchorA;
    [SerializeField] private Transform anchorB;
    [SerializeField] GameObject securityFloor;

    public int moveChecks = 0;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        diceHand = new List<Dice>();
        diceBank = new List<Dice>();

        for (int i = 0; i < diceBankTransform.childCount; i++)
        {
            diceBank.Add(diceBankTransform.GetChild(i).GetComponent<Dice>());
            diceBankTransform.GetChild(i).GetComponent<Dice>().diceManager = this;
        }

        AddDice(startDiceNbr);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollTheDice();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            foreach (var dice in diceHand)
                dice.ChangeState(States.Idle);
        }
    }

    public void AddDice(int nbrOfDice)
    {
        for (int i = 0; i < nbrOfDice; i++)
        {
            if (diceBank.Count > 0)
            {
                int random = Random.Range(0, diceBank.Count);

                diceHand.Add(diceBank[random]);

                print("add " + diceBank[random] + " to player's dice pool");

                diceBank.Remove(diceBank[random]);
            }
            else
            {
                print("no more dice in the bank");
            }
            
        }
    }

    IEnumerator ForceFaceCheck()
    {
        yield return new WaitForSeconds(6f);
        //CallFaceCheck();
    }

    private void RollTheDice()
    {
        securityFloor.SetActive(true);
        print("Take your chance, roll the dice !");

        moveChecks = 0;

        for (int i = 0; i < diceHand.Count; i++)
        {
            float randomX = Random.Range(anchorA.position.x, anchorB.position.x);
            float randomZ = Random.Range(anchorA.position.z, anchorB.position.z);

            diceHand[i].transform.position = new Vector3(randomX, anchorA.position.y, randomZ);
        }

        for (int i = 0; i < diceHand.Count; i++)
        {
            diceHand[i].ChangeState(States.Roll);
        }

        StartCoroutine(ForceFaceCheck());
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(anchorA.position, new Vector3(anchorB.position.x, anchorA.position.y, anchorB.position.z));
        Debug.DrawLine(anchorA.position, new Vector3(anchorA.position.x, anchorA.position.y, anchorB.position.z));
        Debug.DrawLine(anchorA.position, new Vector3(anchorB.position.x, anchorA.position.y, anchorA.position.z));
        Debug.DrawLine(new Vector3(anchorB.position.x, anchorA.position.y, anchorB.position.z), new Vector3(anchorA.position.x, anchorA.position.y, anchorB.position.z));
        Debug.DrawLine(new Vector3(anchorB.position.x, anchorA.position.y, anchorB.position.z), new Vector3(anchorB.position.x, anchorA.position.y, anchorA.position.z));
    }

    public void AddMoveCheck()
    {
        moveChecks++;

        if (moveChecks >= diceHand.Count)
            CallFaceCheck();
    }

    public void RemoveMoveCheck()
    {
        moveChecks--;
    }

    private void CallFaceCheck()
    {
        StopAllCoroutines();
        for (int i = 0; i < diceHand.Count; i++)
        {
            diceHand[i].CheckFaces();

            diceHand[i].ChangeState(States.Display);
        }

        securityFloor.SetActive(false);
    }
}
