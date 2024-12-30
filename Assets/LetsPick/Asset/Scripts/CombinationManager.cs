using System.Collections.Generic;
using UnityEngine;

public class CombinationManager : MonoBehaviour
{
    // �L�ƃJ�b�v�̎�ނ��`����Enum
    public enum CatType { Brown, White, Hachi, Melon }
    public enum CupType { Blue, Red, Green, Yellow }

    // �L�ƃJ�b�v�̗L���ȑg�ݍ��킹���Ǘ�����Dictionary
    private Dictionary<CatType, CupType> validCombinations;

    void Start()
    {
        // �L���ȑg�ݍ��킹���`
        validCombinations = new Dictionary<CatType, CupType>
        {
            { CatType.Brown, CupType.Yellow }, // Brown�L��Yellow�J�b�v
            { CatType.White, CupType.Red },    // White�L��Red�J�b�v
            { CatType.Hachi, CupType.Blue },   // Hachi�L��Blue�J�b�v
            { CatType.Melon, CupType.Green }   // Melon�L��Green�J�b�v
        };
    }

    // �g�ݍ��킹���L�������肷�郁�\�b�h
    public bool IsValidCombination(CatType cat, CupType cup)
    {
        return validCombinations.ContainsKey(cat) && validCombinations[cat] == cup;
    }
}
