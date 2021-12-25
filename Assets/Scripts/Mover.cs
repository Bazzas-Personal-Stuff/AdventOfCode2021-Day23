using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Mover : MonoBehaviour {
    public enum ObjType {
        a,
        b,
        c,
        d
    }

    public ObjType objType;
    public TextMeshPro objText;
    private int[] costs = { 1, 10, 100, 1000 };
    private Vector3 originalPos;
    
    public int cost;

    // Start is called before the first frame update
    void Start() {
        objText = GetComponent<TextMeshPro>();
        UpdateType(objType);
        originalPos = transform.position;

    }
    

    public void UpdateType(ObjType newType) {
        switch (newType) {
            case ObjType.a:
                cost = costs[0];
                objText.text = "A";
                break;
            case ObjType.b:
                cost = costs[1];
                objText.text = "B";
                break;
            case ObjType.c:
                cost = costs[2];
                objText.text = "C";
                break;
            case ObjType.d:
                cost = costs[3];
                objText.text = "D";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        objType = newType;
    }

    public void Reset() {
        transform.position = originalPos;
    }
    void OnMouseDown() {
        GameManager.selectedMover = this;
    }
}