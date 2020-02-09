using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    List<IInteractable> availableObjects = new List<IInteractable>();
    [SerializeField]
    IInteractable currentObject = null;

    float interactionTime = 1f;

    [SerializeField]
    LayerMask interactLayer = new LayerMask();

    public Transform cam = null;

    public float interactionDistance = 5f;

    bool interacting;

    private void Awake()
    {
        availableObjects = new List<IInteractable>();
        InputManager.manager.AddEvent("Interact", OnStartInteract, InputType.OnStarted);
        InputManager.manager.AddEvent("Interact", OnContinueInteract, InputType.OnPerformed);
        InputManager.manager.AddEvent("Interact", OnEndInteract, InputType.OnCancelled);
    }

    void Update()
    {
        RaycastHit hit;
        if(Physics.BoxCast(cam.position, new Vector3(0.2f, 0.2f, 0.2f), cam.forward, out hit, cam.rotation, interactionDistance, interactLayer))
        {
            Debug.DrawLine(cam.position, hit.point);
            availableObjects.Clear();
            if (hit.collider.CompareTag("Interactable"))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null && !availableObjects.Contains(interactable))
                {
                    availableObjects.Add(interactable);
                }
            }
        }
        else if(availableObjects.Count > 0)
        {
            availableObjects.Clear();
        }

        if (availableObjects.Count > 0)
        {
            Debug.DrawLine(cam.position, availableObjects[0].GetTransform().position);
            InteractionUIManager.manager.UpdateHint(availableObjects[0].GetTransform().position);
        }
        else
        {
            InteractionUIManager.manager.CloseHint();
        }

        if (interacting)
            OnContinueInteract();
    }

    void OnContinueInteract ()
    {
        if (currentObject != null)
        {
            InteractionUIManager.manager.UpdateInteract(interactionTime / currentObject.InteractTime, currentObject.GetTransform().position);

            interactionTime += Time.deltaTime;
            if (interactionTime >= currentObject.InteractTime)
            {
                currentObject.OnFinishInteract(this);
                InteractionUIManager.manager.CloseInteract();
                interactionTime = 0;
                currentObject = null;
            }
        }
    }

    void OnStartInteract ()
    {
        interacting = true;
        if (availableObjects.Count > 0 && currentObject == null)
        {
            currentObject = availableObjects[0];
            currentObject.OnStartInteract(this);
            interactionTime = 0;
            availableObjects.RemoveAt(0);
            InteractionUIManager.manager.CloseHint();
        }
    }

    void OnEndInteract ()
    {
        interacting = false;
        if (currentObject != null)
        {
            InteractionUIManager.manager.CloseInteract();
            availableObjects.Insert(0, currentObject);
            currentObject = null;
        }
    }
}
