using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrounndPiece : MonoBehaviour
{
    public bool iscolor = false;

    public void ChangeColored(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
        iscolor = true;
        GameManager.singelton.CheckComplete();

    }

}
