/*using Controls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandControl : MonoBehaviour
{
    //HandStates 
    public enum State
    {
        EMPTY,
        TOUCHING,
        HOLDING
    };
    public State HandState = State.EMPTY;
    private State PrevHandState = State.EMPTY;


    private List<IInteractableObject> _interactableObjects;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateHoverColor()
    {
        _meshRenderer.material.color = _interactableObjects.Count == 0 ? defaultColor : hoverColor;
    }

    // Update is called once per frame
    void Update()
    {
        while (_interactableObjects.Count > 0 && _interactableObjects[_interactableObjects.Count - 1] == null)
        {
            _interactableObjects.RemoveAt(_interactableObjects.Count - 1);
            UpdateHoverColor();
        }
        switch (HandState)
        {
            case State.TOUCHING:
                if (_interactableObjects.Count > 0 &&
                    OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) >= 0.5f)
                {
                    _interactableObjects[_interactableObjects.Count - 1].StartInteraction(controllerSphereCollider, this);
                    currentInteraction = _interactableObjects[_interactableObjects.Count - 1];
                    HandState = State.HOLDING;
                }

                break;
            case State.HOLDING:

                if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Controller) < 0.5f)
                {
                    currentInteraction.EndInteraction(this);
                    if (currentInteraction != null && !_interactableObjects.Contains(currentInteraction))
                    {
                        currentInteraction.EndHighlight();
                    }

                    // clean up deleted objects
                    _interactableObjects.RemoveAll(obj => (obj == null));

                    HandState = _interactableObjects.Count == 0 ? State.EMPTY : State.TOUCHING;
                    controls.DestroyInvalidObjects();
                    controls.UpdateControls();
                    UpdateHoverColor();
                    currentInteraction = null;
                }

                break;
        }

        //Debug state changes
        if (PrevHandState != HandState)
        {
            //Debug.Log(Controller.ToString() +": " + HandState.ToString());
        }
        PrevHandState = HandState;

    }



}
*/