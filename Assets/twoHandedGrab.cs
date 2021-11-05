using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System.Collections.Generic;


public class twoHandedGrab : XRGrabInteractable
{

    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();

    private XRBaseInteractor secondInteractor;



    [System.Obsolete]
    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
            // item.onSelectEnter.AddListener(OnSecondHandGrab);
            // item.onSelectExit.AddListener(OnSecondHandRelease);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)

    {

        if (secondInteractor && selectingInteractor)
        {
            selectingInteractor.attachTransform.rotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }



        base.ProcessInteractable(updatePhase);

    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("Second Hand Grabbed!");
        secondInteractor = interactor;
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("Second Hand Released");
        secondInteractor = null;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("First Hand Grab");
        base.OnSelectEntered(args);

    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("First Hand Released");
        base.OnSelectExited(args);
        secondInteractor = null;
    }



    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);

        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }
}
