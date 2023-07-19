using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StorageContainer : MonoBehaviour
{
    [Header("Storage")]
    public List<Painting> itemsToStore;
    public GameObject storedObject;
    public TMP_Text storageText;
    private int storageCount;
    private bool levelComplete;
    [Header("Controls")]
    // public InputActionReference IAGoToLevel1;
    public InputActionReference IARestartLevel;
    public InputActionReference IAQuit;

    void Start()
    {
        levelComplete = false;
        storageCount = 0;
        storageText.text = "Paintings Stored:\r\n" + storageCount + " / " + itemsToStore.Count;
        /*
        IAGoToLevel1.action.Enable();
        IAGoToLevel1.action.performed += (ctx) =>
        {
            SceneManager.LoadScene("Scene 1");
        };
        */
        IARestartLevel.action.Enable();
        IARestartLevel.action.performed += (ctx) =>
        {
            SceneManager.LoadScene("Scene 2");
        };

        IAQuit.action.Enable();
        IAQuit.action.performed += (ctx) =>
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        };
    }

    void Update()
    {
        if (!levelComplete)
        {
            if (storageCount < itemsToStore.Count)
            {
                if (storedObject)
                {
                    if (!storedObject.GetComponent<GrabbableObject>().IsGrabbed())
                    {
                        if (storedObject.transform.rotation.eulerAngles.x >= 265f || storedObject.transform.rotation.eulerAngles.x <= 275f)
                        {
                            storageCount++;
                            storageText.text = "Paintings Stored:\r\n" + storageCount + " / " + itemsToStore.Count;
                            Destroy(storedObject);
                        }
                    }
                }
            }
            else
            {
                storageText.text = "Congrats!\r\n\r\nAll Paintings have been Stored!";
                levelComplete = true;
            }
        }   
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other != null && other.gameObject.CompareTag("Grabbable"))
        {
            storedObject = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other != null && other.gameObject.CompareTag("Grabbable"))
        {
            storedObject = null;
        }
    }

}
