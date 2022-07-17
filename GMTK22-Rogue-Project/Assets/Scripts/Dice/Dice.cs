using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colors
{
    Red, Blue, Yellow
}
public enum States
{
    Idle,
    Roll,
    Wait,
    Display

}

public class Dice : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Texture2D texture;
    [SerializeField] private Rigidbody rbody;

    public int value; 
    Colors[] colors;

    [SerializeField] private float minImpulse;
    [SerializeField] private float maxImpulse;

    [SerializeField] private float minTorque;
    [SerializeField] private float maxTorque;

    [HideInInspector] public DiceManager diceManager;
    [SerializeField] private States currentState = States.Idle;
    private MaterialPropertyBlock propBlock;
    private bool isMoving = true;

    private void Start()
    {
        propBlock = new MaterialPropertyBlock();

        ApplyTexture();
    }

    private void Update()
    {
        if (currentState == States.Roll)
        {
            if (rbody.velocity.magnitude <= 0f && rbody.angularVelocity.magnitude <= 0f && isMoving)
            {
                isMoving = false;

                diceManager.AddMoveCheck();
            }
            else if ((rbody.velocity.magnitude > 0f || rbody.angularVelocity.magnitude > 0f) && !isMoving)
            {
                isMoving = true;
                diceManager.RemoveMoveCheck();
            }
        }
    }

    [ContextMenu("ApplyTexture")]
    private void ApplyTexture()
    {
        //Recup Data
        meshRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetTexture("_BaseMap", texture);
        //Push Data
        meshRenderer.SetPropertyBlock(propBlock);
    }

    public void ChangeState(States state)
    {
        switch (state)
        {
            case States.Idle:
                break;
            case States.Roll:
                rbody.constraints = RigidbodyConstraints.None;
                rbody.useGravity = true;

                rbody.AddForce(Random.insideUnitSphere.normalized * Random.Range(minImpulse, maxImpulse), ForceMode.Impulse);
                rbody.AddTorque(Random.insideUnitSphere.normalized * Random.Range(minTorque, maxTorque), ForceMode.Impulse);
                isMoving = true;

                
                break;
            case States.Display:
                break;

            default:
                break;
        }

        currentState = state;
    }

}
