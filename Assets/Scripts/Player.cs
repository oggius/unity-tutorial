using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectHolder {

    public static Player Instance { get; private set; }

    public event EventHandler<SelectedCounterChangedEventArgs> selectedCounterChanged;
    public class SelectedCounterChangedEventArgs {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 6f;
    private float rotateSpeed = 15f;
    private float playerRadius = .7f;
    private float playerHeight = 2f;

    private Vector3 lastInteractDirection;

    private ClearCounter selectedCounter;

    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;

    private KitchenObject heldKitchenObject;
    [SerializeField] private GameObject holdingPoint;

    private void Awake()
    {
        if (Instance != null) {
            Debug.LogError("There are multiple player instances");
        }

        Instance = this;
        Debug.Log("Instance assigned");
    }

    private void Start()
    {
        gameInput.interactAction += GameInput_interactAction;
    }

    private void GameInput_interactAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    // returns true if the player is currently walking
    public bool IsWalking()
    {
        return isWalking;
    }

    // handles movement logic
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // explicit cast to transform.position's Vector3 type
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        // rotate player to corresponding direction
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float moveDistance = moveSpeed * Time.deltaTime;

        // use physics to check if there's collision with different object
        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDirection,
            moveDistance
            );

        if (!canMove) {
            // can not move towards current direction

            // attempt only X direction
            Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0);
            canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirX,
                moveDistance
            );

            if (canMove) {
                moveDirection = moveDirX;
            } else {
                // attempt only Z direction
                Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z);
                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirZ,
                    moveDistance
                );

                if (canMove) {
                    moveDirection = moveDirZ;
                }
            }
        }
        if (canMove) {
            // change position (move)
            transform.position += moveDirection * moveDistance;
        }

        // detect if the player is walking
        isWalking = moveDirection != Vector3.zero;
    }

    // handles interactions logic
    private void HandleInteractions() {
        float interactionDistance = 2f;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // explicit cast to transform.position's Vector3 type
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDirection != Vector3.zero) {
            lastInteractDirection = moveDirection;
        }

        // TODO: get deeper into Layers concept
        RaycastHit interactedObject = new RaycastHit();
        if (Physics.Raycast(transform.position, lastInteractDirection, out interactedObject, interactionDistance, counterLayerMask)) {
            if (interactedObject.transform.TryGetComponent(out ClearCounter clearCounter)) {
                if (selectedCounter != clearCounter) {
                    SetSelectedCounter(clearCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    // sets selected counter
    private void SetSelectedCounter(ClearCounter counter) {
        selectedCounter = counter;

        selectedCounterChanged?.Invoke(this, new SelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }

    public void ObtainKitchenObject(KitchenObject kitchenObject)
    {
        this.heldKitchenObject = kitchenObject;
    }

    public KitchenObject GetHeldObject()
    {
        return heldKitchenObject;
    }

    public void ReleaseHeldObject()
    {
        heldKitchenObject = null;
    }

    public bool HasHeldObject()
    {
        return heldKitchenObject == null;
    }

    public GameObject GetHoldingPoint()
    {
        return GetHoldingPoint();
    }
}
