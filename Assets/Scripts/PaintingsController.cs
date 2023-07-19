using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PaintingsController : MonoBehaviour
{
    [Header("Painting Properties:")]
    public List<GameObject> paintings;
    public Transform paintingHolderLeft;
    public Transform paintingHolderRight;
    public Transform realPaintingPile;
    public Transform fakePaintingPile;

    [Header("UI")]
    public GameObject instructionsPanel;
    public GameObject instructionsSidePanel;
    public GameObject controlsPanel;
    public GameObject controlsSidePanel;
    public TMP_Text scoreText;

    [Header("Input Actions")]
    public InputActionReference aLeftPainting;
    public InputActionReference aRightPainting;
    public InputActionReference aStart;
    public InputActionReference aNextPainting;
    public InputActionReference aGoToScene2;
    public InputActionReference aQuit;

    private int paintingIndex = 0;
    private int authenticScore = 0;
    private bool hasGameStarted = false;

    void Start()
    {
        instructionsPanel.SetActive(true);
        controlsPanel.SetActive(true);
        instructionsSidePanel.SetActive(false);
        controlsSidePanel.SetActive(false);
        scoreText.text = "";
        hasGameStarted = false;
}

    void Update()
    {
        // Select Left Painting
        aLeftPainting.action.Enable();
        aLeftPainting.action.performed += (ctx) =>
        {
            if (paintingHolderLeft.childCount != 0 && paintingHolderRight.childCount != 0)
            {
                // Check if user selected the Authentic painting
                if (paintingHolderLeft.GetChild(0).gameObject.GetComponent<Painting>().isRealPainting)
                    authenticScore++;
                paintingHolderLeft.GetChild(0).SetParent(realPaintingPile);
                realPaintingPile.GetChild(realPaintingPile.childCount - 1).transform.SetLocalPositionAndRotation(new Vector3(0f, realPaintingPile.childCount * 0.05f, 0f), Quaternion.Euler(-90f, 0f, 0f));
                paintingHolderRight.GetChild(0).SetParent(fakePaintingPile);
                fakePaintingPile.GetChild(fakePaintingPile.childCount - 1).transform.SetLocalPositionAndRotation(new Vector3(0f, fakePaintingPile.childCount * 0.05f, 0f), Quaternion.Euler(-90f, 0f, 0f));

                scoreText.text = "Authentic Paintings Found:\r\n\r\n" + authenticScore + " / " + paintings.Count;
            }
        };

        // Select Right Painting
        aRightPainting.action.Enable();
        aRightPainting.action.performed += (ctx) =>
        {
            if (paintingHolderLeft.childCount != 0 && paintingHolderRight.childCount != 0)
            {
                // Check if user selected the Authentic painting
                if (paintingHolderRight.GetChild(0).gameObject.GetComponent<Painting>().isRealPainting)
                    authenticScore++;
                paintingHolderRight.GetChild(0).SetParent(realPaintingPile);
                realPaintingPile.GetChild(realPaintingPile.childCount - 1).transform.SetLocalPositionAndRotation(new Vector3(0f, realPaintingPile.childCount * 0.05f, 0f), Quaternion.Euler(-90f, 0f, 0f));
                paintingHolderLeft.GetChild(0).SetParent(fakePaintingPile);
                fakePaintingPile.GetChild(fakePaintingPile.childCount - 1).transform.SetLocalPositionAndRotation(new Vector3(0f, fakePaintingPile.childCount * 0.05f, 0f), Quaternion.Euler(-90f, 0f, 0f));

                scoreText.text = "Authentic Paintings Found:\r\n\r\n" + authenticScore + " / " + paintings.Count;
            }
        };

        // Go to Next Painting pair
        aNextPainting.action.Enable();
        aNextPainting.action.performed += (ctx) =>
        {
            if (paintingHolderLeft.childCount == 0 || paintingHolderRight.childCount == 0) 
                NextPainting();
        };

        // Start investigating
        aStart.action.Enable();
        aStart.action.performed += (ctx) =>
        {
            if ((paintingHolderLeft.childCount == 0 || paintingHolderRight.childCount == 0) && (realPaintingPile.childCount == 0 || fakePaintingPile.childCount == 0))
                StartScene1();
            else
                SceneManager.LoadScene("Scene 1");
        };

        // Go to Scene 2
        aGoToScene2.action.Enable();
        aGoToScene2.action.performed += (ctx) =>
        {
            SceneManager.LoadScene("Scene 2");
        };

        // Quit
        aQuit.action.Enable();
        aQuit.action.performed += (ctx) =>
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                            Application.Quit();
            #endif
        };
    }

    public void StartScene1()
    {
        instructionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        instructionsSidePanel.SetActive(true);
        controlsSidePanel.SetActive(true);

        paintings = paintings.OrderBy(i => System.Guid.NewGuid()).ToList();
        int randPos = Random.Range(0, 2);
        if (randPos == 0)
        {
            paintings[paintingIndex].GetComponent<Painting>().isRealPainting = true;
            GameObject p1 = Instantiate(paintings[paintingIndex], paintingHolderLeft);
            p1.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            p1.SetActive(true);

            paintings[paintingIndex].GetComponent<Painting>().isRealPainting = false;
            GameObject p2 = Instantiate(paintings[paintingIndex], paintingHolderRight);
            p2.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            p2.SetActive(true);
        }
        else
        {
            paintings[paintingIndex].GetComponent<Painting>().isRealPainting = false;
            GameObject p1 = Instantiate(paintings[paintingIndex], paintingHolderLeft);
            p1.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            p1.SetActive(true);

            paintings[paintingIndex].GetComponent<Painting>().isRealPainting = true;
            GameObject p2 = Instantiate(paintings[paintingIndex], paintingHolderRight);
            p2.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
            p2.SetActive(true);
        }

        foreach (GameObject p in paintings)
        {
            p.SetActive(false);
        }

        hasGameStarted = true;
    }

    public void NextPainting()
    {
        instructionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        instructionsSidePanel.SetActive(true);
        controlsSidePanel.SetActive(true);

        paintingIndex++;
        if (paintingIndex >= paintings.Count)
        {
            scoreText.text = "Level Complete!\r\n\r\nAuthentic Paintings Found:\r\n\r\n" + authenticScore + " / " + paintings.Count;
        } 
        else
        {
            if (hasGameStarted)
            {
                int randPos = Random.Range(0, 2);
                if (randPos == 0)
                {
                    paintings[paintingIndex].GetComponent<Painting>().isRealPainting = true;
                    GameObject p1 = Instantiate(paintings[paintingIndex], paintingHolderLeft);
                    p1.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
                    p1.SetActive(true);

                    paintings[paintingIndex].GetComponent<Painting>().isRealPainting = false;
                    GameObject p2 = Instantiate(paintings[paintingIndex], paintingHolderRight);
                    p2.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
                    p2.SetActive(true);
                }
                else
                {
                    paintings[paintingIndex].GetComponent<Painting>().isRealPainting = false;
                    GameObject p1 = Instantiate(paintings[paintingIndex], paintingHolderLeft);
                    p1.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
                    p1.SetActive(true);

                    paintings[paintingIndex].GetComponent<Painting>().isRealPainting = true;
                    GameObject p2 = Instantiate(paintings[paintingIndex], paintingHolderRight);
                    p2.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f));
                    p2.SetActive(true);
                }
            }
        }

        foreach (GameObject p in paintings)
        {
            p.SetActive(false);
        }
    }
}
