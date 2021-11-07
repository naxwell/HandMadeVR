using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class objectDuplicator : MonoBehaviour
{

    [SerializeField] private InputActionReference activateReference = null;
    private XRRayInteractor interactor;

    public GameObject objToCopy;
    // Start is called before the first frame update
    void Start()
    {
        interactor = GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        activateReference.action.started += SetObject;
        activateReference.action.canceled += DuplicateObject;
    }

    private void OnDisable()
    {
        activateReference.action.started -= SetObject;
        activateReference.action.canceled -= DuplicateObject;
    }

    void DuplicateObject(InputAction.CallbackContext ctx)
    {
        if (objToCopy.CompareTag("duplication"))
        {
            Instantiate(objToCopy, transform.position, transform.rotation);
        }

        //objToCopy = null;
    }

    void SetObject(InputAction.CallbackContext ctx)
    {
        objToCopy = interactor.selectTarget.gameObject;


    }

    public void objectSet(HoverEnterEventArgs args)
    {
        List<XRBaseInteractable> targets = new List<XRBaseInteractable>();
        args.interactor.GetHoverTargets(targets);
        Debug.Log(targets);
        objToCopy = targets[0].gameObject;
    }
}
