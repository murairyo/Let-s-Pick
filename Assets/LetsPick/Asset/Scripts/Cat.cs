using UnityEngine;

public class Cat : MonoBehaviour
{
    public CombinationManager.CatType catType; // CombinationManagerのCatTypeを使用
}
public enum CatType
{
    Brown,  // 茶色
    Hachi,  // ハチワレ
    Melon,  // メロン
    White   // 白猫
}
