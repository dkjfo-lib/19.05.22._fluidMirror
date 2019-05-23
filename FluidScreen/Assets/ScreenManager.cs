using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    private void Awake()
    {
        instance = this;
    }

    // refs 
    public InteractiveImage intImg_prefab;
    public RectTransform imgHolderRect;

    // props
    [Range(0.05f, 1f)] public float offsetX = 0.8f;
    [Range(0.05f, 1f)] public float offsetY = 0.8f;
    public Sprite spriteImg;
    [Range(0.025f, 1f)] public float imgSize = 0.5f;

    [Header("Controll values")]
    public Color color1 = Color.white;
    public Color color2 = Color.red;
    [Range(0.5f, 6f)] public float maxRotationsPerSecond = 2f;
    [Range(1f, 5f)] public float maxSecondsToRotate = 4f;

    void Start()
    {
        CreateGrid();
    }

    public void myUpdate()
    {
        DeleteGrid();
        CreateGrid();
    }

    void CreateGrid()
    {
        if (spriteImg == null)
            return;

        Vector2 oneImgSize = spriteImg.rect.size * imgSize;
        float deltaWidth = oneImgSize.x * offsetX;
        float deltaHeigth = oneImgSize.y * offsetY;

        for (float x = -deltaWidth; x < imgHolderRect.rect.width + deltaWidth; x += deltaWidth)
        {
            for (float y = -deltaHeigth; y < imgHolderRect.rect.height + deltaHeigth; y += deltaHeigth)
            {
                Instantiate(
                intImg_prefab,
                imgHolderRect).
                setSprite(new Vector3(x, y, 0), spriteImg, oneImgSize, color1, color2);
            }
        }
    }
    void DeleteGrid()
    {
        for (int i = 0; i < imgHolderRect.childCount; i++)
        {
            Destroy(imgHolderRect.GetChild(i).gameObject);
        }
    }
}
