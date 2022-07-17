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
    Display
}

public class Dice : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Texture2D texture;
    [SerializeField] private Rigidbody rbody;

    public int value; 
    public Colors[] colors;

    [SerializeField] private float minImpulse;
    [SerializeField] private float maxImpulse;

    [SerializeField] private float minTorque;
    [SerializeField] private float maxTorque;

    [SerializeField] private float faceCheckRadius = 0.1f;
    [SerializeField] private LayerMask faceChecklayerMask;

    [HideInInspector] public DiceManager diceManager;
    [SerializeField] private States currentState = States.Idle;
    private MaterialPropertyBlock propBlock;
    private bool isMoving = true;
    private bool hasHitGroundOnce = false; //Ceci est un vieux pensement qui sent le cul mais promi c'est la faute de Unity

    private void Start()
    {
        propBlock = new MaterialPropertyBlock();

        ApplyTexture();
    }

    private void Update()
    {
        if (currentState == States.Roll)
        {
            if (rbody.velocity.magnitude <= 0f && rbody.angularVelocity.magnitude <= 0f && isMoving && hasHitGroundOnce)
            {
                isMoving = false;

                diceManager.AddMoveCheck();
            }
            else if ((rbody.velocity.magnitude > 0f || rbody.angularVelocity.magnitude > 0f) && !isMoving && hasHitGroundOnce)
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
                rbody.constraints = RigidbodyConstraints.FreezeRotation;

                isMoving = false;
                hasHitGroundOnce = false;
                break;

            default:
                break;
        }

        currentState = state;
    }

    public void CheckFaces()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (Physics.CheckSphere(transform.GetChild(i).position, faceCheckRadius, faceChecklayerMask))
            {
                value = i + 1;
                print(this.name + " rolled a " + value);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7 && !hasHitGroundOnce)
        {
            hasHitGroundOnce = true;
        }
    }
}
