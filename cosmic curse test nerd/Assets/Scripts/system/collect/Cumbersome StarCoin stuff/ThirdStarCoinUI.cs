using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThirdStarCoinUI : MonoBehaviour
{
    public Sprite notGotten;
    public Sprite gottenCoin;
    Image spriteImage;
    public StarIndex starIndex;

    [Header("Change for star coin index")]
    [SerializeField] int coinIndex;
    private void OnEnable()
    {
        StarCoin.ThreeStarCollected += GotStarCoin;
    }
    private void OnDisable()
    {
        StarCoin.ThreeStarCollected -= GotStarCoin;
    }

    private void Start()
    {
        spriteImage = GetComponent<Image>();
        spriteImage.sprite = notGotten;
    }
    private void GotStarCoin()
    {
        if (coinIndex == starIndex.StarCoinNumber)
        {
            spriteImage.sprite = gottenCoin;
        }
        else if (coinIndex != starIndex.StarCoinNumber)
        {
            Debug.LogError("No index match, make sure this one is matching an index");
        }
    }
}
