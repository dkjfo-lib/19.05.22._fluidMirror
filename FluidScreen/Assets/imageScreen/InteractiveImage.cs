using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveImage : MonoBehaviour
{
    // refs
    public Image imgImage;
    public Image holderImage;
    public RectTransform holderRectTransform;
    public RectTransform imgRectTransform;

    // props 
    public Color color1 = Color.white;
    public Color color2 = Color.red;

    // stats
    [Range(0, 1)] public float distance;
    [Range(0, 1)] public float force;
    private float sqrDistance { get { return distance * distance; } }

    void Start()
    {

    }

    void Update()
    {
        UpdateColor();
        UpdateRotation();
    }

    public void setSprite(Vector3 position, Sprite sprite, Vector2 size, Color color1, Color color2)
    {
        holderRectTransform.anchoredPosition = position;
        holderRectTransform.sizeDelta = size;
        imgImage.sprite = sprite;
        this.color1 = color1;
        this.color2 = color2;
    }

    void UpdateColor()
    {
        float sqrMagnitude = Vector3.SqrMagnitude(Input.mousePosition - imgRectTransform.position);
        float sqrRange = CursorShadow.instance.size * CursorShadow.instance.size * 1000;
        distance = sqrMagnitude < sqrRange ? 1 - sqrMagnitude / sqrRange : 0;
        imgImage.color = Color.Lerp(color1, color2, sqrDistance);
        holderImage.color = Color.Lerp(color1, color2, sqrDistance);
    }

    void UpdateRotation()
    {
        if (distance > 0f)
        {
            force += 
                CursorShadow.instance.mouseDirection.sqrMagnitude /
                holderRectTransform.rect.size.sqrMagnitude *
                sqrDistance * Time.deltaTime;
            force = force > 1 ? 1 : force;
        }
        if (force > 0f)
        {
            float deltaDegrees = force * ScreenManager.instance.maxRotationsPerSecond * 360 * Time.deltaTime;
            //float valB = -CursorShadow.instance.mouseDirection.x / CursorShadow.instance.mouseDirection.y;
            //imgRectTransform.Rotate(new Vector3(1, valB, 0), deltaDegrees);
            imgRectTransform.Rotate(Vector3.up, deltaDegrees);
            force -= Time.deltaTime / ScreenManager.instance.maxSecondsToRotate;
            force = force < 0 ? 0 : force;
        }
    }
}


static class Gradient
{
    public static Color getGradientColor(Color c1, Color c2, int size, int position)
    {
        float rMax = c1.r;
        float rMin = c2.r;
        float gMin = c1.g;
        float gMax = c2.g;
        float bMin = c1.b;
        float bMax = c2.b;
        // ... and for B, G
        var rAverage = rMin + (rMax - rMin) * position / size;
        var gAverage = gMin + (gMax - gMin) * position / size;
        var bAverage = bMin + (bMax - bMin) * position / size;
        return new Color(rAverage, gAverage, bAverage);
    }
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
