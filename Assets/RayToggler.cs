using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


[RequireComponent(typeof(XRRayInteractor))]
public class RayToggler : MonoBehaviour
{

    [SerializeField] private InputActionReference activateReference = null;
    private XRRayInteractor rayInteractor = null;
    private bool isEnabled = false;

    private void Awake()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    private void OnEnable()
    {
        activateReference.action.started += ToggleRay;
        activateReference.action.canceled += ToggleRay;
    }

    private void OnDisable()
    {
        activateReference.action.started -= ToggleRay;
        activateReference.action.canceled -= ToggleRay;
    }


    private void ToggleRay(InputAction.CallbackContext ctx)
    {
        isEnabled = ctx.control.IsPressed();

    }

    private void LateUpdate()
    {
        ApplyStatus();
    }

    private void ApplyStatus()
    {

        if (rayInteractor.enabled != isEnabled)
        {
            rayInteractor.enabled = isEnabled;
        }
    }
}
