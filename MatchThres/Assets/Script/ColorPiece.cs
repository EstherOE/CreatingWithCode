    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class ColorPiece : MonoBehaviour
{
    public enum ColorType
    {
        Blue,
        Green,
        Orange,
        Purple,
        Red,
        Yellow,
        Choco,
        Any,
        Count
    };
    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color;
        public Sprite sprite;
    };

    public ColorSprite[] sprites;

    private ColorType color;
    public ColorType Color
    {
        get { return color; }
        set { SetColor(value); }

    }

    public int NumColors
    {
        get
        {

            return sprites.Length;
        }

    }
    private Dictionary<ColorType, Sprite> spriteDict; // for fast loop

    // Start is called before the first frame update

    private SpriteRenderer sprite;


    private void Awake()
    {


        sprite = transform.Find("piece").GetComponent<SpriteRenderer>();
        spriteDict = new Dictionary<ColorType, Sprite>();

        for (int i = 0; i < sprites.Length; i++)
        {
            if (!spriteDict.ContainsKey(sprites[i].color))
            {
                spriteDict.Add(sprites[i].color, sprites[i].sprite);
            }
        }
    }


    // Update is called once per frame
    public void SetColor(ColorType newColor)
    {
        color = newColor;
        if (spriteDict.ContainsKey(color))
        {
            sprite.sprite = spriteDict[newColor];
        }



    }


}
