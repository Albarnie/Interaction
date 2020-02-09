using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    float InteractTime
    {
        get;
    }

    void OnStartInteract(Interaction user);

    void OnFinishInteract(Interaction user);

    Transform GetTransform();
}
