using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorShadow : MonoBehaviour
{
    public static CursorShadow instance;
    private void Awake()
    {
        instance = this;
    }

    [Range(2.5f, 10f)] public float size = 5f;
    [HideInInspector] public Vector3 mousePosition;
    [HideInInspector] public Vector3 mouseDirection;

    void Start()
    {
    }

    void Update()
    {
        mouseDirection = Input.mousePosition - mousePosition;
        mousePosition = Input.mousePosition;
    }
}
