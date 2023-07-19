using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour
{
    public Material pictureMaterial;
    public Material signatureMaterial;
    public List<Vector3> signaturePositions;
    public GameObject picture;
    public GameObject signature;
    public bool isRealPainting;

    void Awake()
    {
        picture.GetComponent<MeshRenderer>().material = pictureMaterial;
        signature.GetComponent<MeshRenderer>().material = signatureMaterial;
        int randPos = Random.Range(0, signaturePositions.Count);
        signature.transform.localPosition = signaturePositions[randPos];
        signature.SetActive(isRealPainting);
    }
}
