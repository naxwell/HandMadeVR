using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class handPresence : MonoBehaviour
{

    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;

    public GameObject handPrefab;
    private GameObject spawnedHandModel;
    private Animator handAnimator;
    // Start is called before the first frame update
    void Start()
    {



        spawnedHandModel = Instantiate(handPrefab, transform);
        handAnimator = spawnedHandModel.GetComponentInChildren<Animator>();


    }

    // Update is called once per frame
    void Update()
    {

        if (!targetDevice.isValid)
        {
            InitializeInput();
        }

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonVal);
        if (primaryButtonVal)
        {
            Debug.Log("Primary button pressed!");
        }

        UpdateHandAnimation();


    }
    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }



    }

    void InitializeInput()
    {

        List<InputDevice> devices = new List<InputDevice>();
        //InputDevices.GetDevices(devices);
        //InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        Debug.Log(targetDevice);
    }
}
