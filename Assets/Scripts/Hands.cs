using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Hands : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefab;
    public GameObject handModalPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModal;
    private Animator handAnimatior;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>(); // Generatas a list of all input devices (Controllers)
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0) // check the number of devices is grater then 0
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefab.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("Did not find a corresponding controller modal");
                spawnedController = Instantiate(controllerPrefab[0], transform);
            }

            spawnedHandModal = Instantiate(handModalPrefab, transform);
            handAnimatior = spawnedHandModal.GetComponent<Animator>();
        }
    }

    void UpateHandAnimation()
    {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimatior.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimatior.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimatior.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimatior.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (showController)
            {
                spawnedHandModal.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModal.SetActive(true);
                spawnedController.SetActive(false);
                UpateHandAnimation();
            }
        }
    }
}
