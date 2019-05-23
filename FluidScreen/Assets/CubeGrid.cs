using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGrid : MonoBehaviour
{
    public static CubeGrid instance;
    private void Awake()
    {
        instance = this;
    }

    public Camera targetCamera;
    public int maxCubesNumb = 10000;
    public InteractiveCube cube_prefab;
    public int size = 30;

    public Vector3 mousePosition;
    public float maxDistance = 50;
    public int hPlus = 2;
    public Color colorDefault = new Color(32/255f, 0 / 255f, 44 / 255f); // Color.white;
    public Color colorClose = new Color(203 / 255f, 180 / 255f, 212 / 255f); // Color.red;

    public float normalInfluenceSpeed = 0.125f;
    public float fastInfluenceSpeed = 0.25f;
    public float influenceSpeed { get { return fast ? fastInfluenceSpeed : normalInfluenceSpeed; } }

    public bool allUp = false;
    public bool allDown = false;
    public bool fast = false;

    void Start()
    {
        CreateGrid();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
            mousePosition.y = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            fast = !fast;
        }
        if (Input.GetMouseButtonDown(1))
        {
            allUp = !allUp;
            allDown = false;
        }
        if (Input.GetMouseButtonDown(2))
        {
            allDown = !allDown;
            allUp = false;
        }
    }

    void CreateGrid()
    {
        for (int y = -size; y < size; y++)
        {
            for (int x = -size; x < size; x++)
            {
                Instantiate(cube_prefab, new Vector3(x, 0, y), cube_prefab.transform.rotation, transform);
            }
        }
    }
}
