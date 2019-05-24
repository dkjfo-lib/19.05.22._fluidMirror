using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CubeGrid : MonoBehaviour
{
    public static CubeGrid instance;
    private void Awake()
    {
        instance = this;
    }

    [Header("System values")]
    public int maxCubesNumb = 10000;
    public InteractiveCube cube_prefab;
    public int size = 30;
    public Texture2D texture;

    [Header("Hosted values")]
    public Vector3 mousePosition;
    public float maxDistance = 50;
    public int hPlus = 2;
    public Color colorDefault = new Color(32 / 255f, 0 / 255f, 44 / 255f); // Color.white;
    public Color colorClose = new Color(203 / 255f, 180 / 255f, 212 / 255f); // Color.red;
    [HideInInspector] public Color[][] colorMap;
    public int colorDeepth = 10;

    [Header("Respond rate")]
    public float normalInfluenceSpeed = 0.125f;
    public float fastInfluenceSpeed = 0.25f;
    public float influenceSpeed { get { return fast ? fastInfluenceSpeed : normalInfluenceSpeed; } }

    [Header("Modes")]
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
        if (texture != null)
        {
            int rilSize = 2 * size;
            int maxDimention = Mathf.Max(texture.width, texture.height);
            float rawDeltaWidth = (float)maxDimention / rilSize;
            int deltaDimention = Mathf.CeilToInt(rawDeltaWidth);
            int biggerMaxDimention = deltaDimention * rilSize;
            int offsetX = (biggerMaxDimention - texture.width) / 2;
            int offsetY = (biggerMaxDimention - texture.height) / 2;

            int goFuckAway = biggerMaxDimention * biggerMaxDimention;
            int pixelsPerSqr = deltaDimention * deltaDimention;
            colorMap = new Color[rilSize * rilSize][];

            Debug.Log("dimentionCubes : " + rilSize);
            Debug.Log("dimentionImg : " + maxDimention);
            Debug.Log("rawDeltaDimention : " + rawDeltaWidth);
            Debug.Log("deltaDimention : " + deltaDimention);
            Debug.Log("biggerSize : " + biggerMaxDimention);
            for (int blockY = 0; blockY < rilSize; blockY++)
            {
                for (int blockX = 0; blockX < rilSize; blockX++)
                {
                    Vector3 colorWeight = Vector3.zero;
                    for (int y = blockY * deltaDimention - offsetY; y < (blockY + 1) * deltaDimention - offsetY; y++)
                    {
                        for (int x = blockX * deltaDimention - offsetX; x < (blockX + 1) * deltaDimention - offsetX; x++)
                        {
                            //Debug.Log(x + " , " + y);
                            goFuckAway--;
                            if (goFuckAway < 0)
                            {
                                Debug.LogError("Thats Wrong!");
                                return;
                            }
                            if (x < 0 || y < 0 || x > texture.width || y > texture.height)
                            {
                                colorWeight += new Vector3(colorClose.r, colorClose.g, colorClose.b);
                                continue;
                            }
                            Color textureColorClose = texture.GetPixel(x, y);
                            colorWeight += new Vector3(textureColorClose.r, textureColorClose.g, textureColorClose.b);
                        }
                    }
                    Color rilColor = new Color(colorWeight.x / pixelsPerSqr, colorWeight.y / pixelsPerSqr, colorWeight.z / pixelsPerSqr);
                    colorMap[blockY * rilSize + blockX] = GetGradient(colorDefault, rilColor, colorDeepth);
                }
            }
        }
        else
        {
            colorMap = new Color[size * 2 * size * 2][];
            for (int y = 0; y < size * 2; y++)
            {
                for (int x = 0; x < size * 2; x++)
                {
                    colorMap[y * size * 2 + x] = GetGradient(colorDefault, colorClose, colorDeepth);
                }
            }
        }
        for (int y = -size; y < size; y++)
        {
            for (int x = -size; x < size; x++)
            {
                Instantiate(cube_prefab, new Vector3(x, 0, y), cube_prefab.transform.rotation, transform);
            }
        }
    }

    public Color GetColor(int id, float percent)
    {
        if (colorMap == null)
        {
            Debug.LogError("IT IS WRONG!");
            return Color.white;
        }
        if (id >= colorMap.Length || id < 0)
        {
            Debug.LogError("IT IS WRONG!");
            return Color.white;
        }
        int colorID = Mathf.RoundToInt(percent * (colorDeepth - 1));
        if (colorID >= colorDeepth || colorID < 0)
        {
            Debug.LogError("IT IS WRONG!");
            return Color.white;
        }
        return colorMap[id][colorID];
    }

    static Color[] GetGradient(Color startColor, Color endColor, int size)
    {
        float rMin = startColor.r;
        float rMax = endColor.r;
        float gMin = startColor.g;
        float gMax = endColor.g;
        float bMin = startColor.b;
        float bMax = endColor.b;
        // ... and for B, G
        var colorList = new List<Color>();
        for (int i = 0; i < size; i++)
        {
            var rAverage = rMin + (rMax - rMin) * i / size;
            var gAverage = gMin + (gMax - gMin) * i / size;
            var bAverage = bMin + (bMax - bMin) * i / size;
            colorList.Add(new Color(rAverage, gAverage, bAverage));
        }
        return colorList.ToArray();
    }
}

static class Gradient
{
    public static Color[] getGradient(Color c1, Color c2, int size)
    {
        float rMax = c1.r;
        float rMin = c2.r;
        float gMin = c1.g;
        float gMax = c2.g;
        float bMin = c1.b;
        float bMax = c2.b;
        // ... and for B, G
        var colorList = new List<Color>();
        for (int i = 0; i < size; i++)
        {
            var rAverage = rMin + (rMax - rMin) * i / size;
            var gAverage = gMin + (gMax - gMin) * i / size;
            var bAverage = bMin + (bMax - bMin) * i / size;
            colorList.Add(new Color(rAverage, gAverage, bAverage));
        }
        return colorList.ToArray();
    }
}