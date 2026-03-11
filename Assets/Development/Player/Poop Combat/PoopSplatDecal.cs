using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PoopSplatDecal : MonoBehaviour
{
    float timer = 2.5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    float tempAlpha;
    [SerializeField] private List<Sprite> sprites = new();
    bool onWall;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CheckIfOnWall();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            FadeImage();
        } else Destroy(gameObject);
    }

    private void FadeImage()
    {
        if (spriteRenderer != null)
        {
            var color = spriteRenderer.color;
            var alpha = color.a;
            var newAlpha = Mathf.Lerp(alpha, 0, 0.1f);
            color.a = newAlpha;
            spriteRenderer.color = color;
            
        }
    }

    private void GetRandomSprite()
    {
        int rand;
        if (onWall)
        {
            rand = 2;
            

        }
        else
        {
            rand = Random.Range(0, 1);
        }

        var sprite = sprites[rand];
        spriteRenderer.sprite = sprite;
    }

    void CheckIfOnWall()
    {
        if(transform.rotation.eulerAngles == Vector3.zero) 
        {
            onWall = true;
        }
        GetRandomSprite();
    }


}
