using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface class that all interactor class should use
public interface IInteractor
{
    void Interact();
    string GetInteractText();
}
