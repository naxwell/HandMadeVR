using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using System.Collections.Generic;


public class twoHandedGrab : XRGrabInteractable
{

    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();

    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitRot;
    public enum TwoHandRotationType { None, First, Second };
    public TwoHandRotationType twoHandRotationType;

    private float initDist;
    private float dropDist;
    public GameObject firstHand;
    public GameObject secondHand;


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
        if (secondInteractor != null)
        {
            GrowObject();
            initDist = Vector3.Distance(firstHand.transform.position, secondHand.transform.position);

        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)

    {

        if (secondInteractor && selectingInteractor)
        {
            selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            //GrowObject();
        }

        base.ProcessInteractable(updatePhase);

    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        }

        return targetRotation;
    }

    private void GrowObject()
    {
        dropDist = Vector3.Distance(firstHand.transform.position, secondHand.transform.position);
        float newScaleMod = dropDist / initDist;
        Debug.Log(newScaleMod);
        transform.localScale = transform.localScale * newScaleMod;
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("Second Hand Grabbed!");
        secondInteractor = interactor;
        secondHand = interactor.gameObject;
        initDist = Vector3.Distance(firstHand.transform.position, secondHand.transform.position);

    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("Second Hand Released");
        GrowObject();
        secondInteractor = null;

    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("First Hand Grab");
        base.OnSelectEntered(args);
        attachInitRot = args.interactor.attachTransform.localRotation;
        firstHand = args.interactor.gameObject;

    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Debug.Log("First Hand Released");
        base.OnSelectExited(args);
        secondInteractor = null;
        args.interactor.attachTransform.localRotation = attachInitRot;
    }



    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);

        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }
}
