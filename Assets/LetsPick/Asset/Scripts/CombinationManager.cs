using System.Collections.Generic;
using UnityEngine;

public class CombinationManager : MonoBehaviour
{
    // 猫とカップの種類を定義するEnum
    public enum CatType { Brown, White, Hachi, Melon }
    public enum CupType { Blue, Red, Green, Yellow }

    // 猫とカップの有効な組み合わせを管理するDictionary
    private Dictionary<CatType, CupType> validCombinations;

    void Start()
    {
        // 有効な組み合わせを定義
        validCombinations = new Dictionary<CatType, CupType>
        {
            { CatType.Brown, CupType.Yellow }, // Brown猫はYellowカップ
            { CatType.White, CupType.Red },    // White猫はRedカップ
            { CatType.Hachi, CupType.Blue },   // Hachi猫はBlueカップ
            { CatType.Melon, CupType.Green }   // Melon猫はGreenカップ
        };
    }

    // 組み合わせが有効か判定するメソッド
    public bool IsValidCombination(CatType cat, CupType cup)
    {
        return validCombinations.ContainsKey(cat) && validCombinations[cat] == cup;
    }
}
