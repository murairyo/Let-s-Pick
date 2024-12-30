using UnityEngine;

public class Cup : MonoBehaviour
{
    // カップの種類を指定するためのEnum
    public enum CupType
    {
        Blue,   // 青色のカップ
        Red,    // 赤色のカップ
        Green,  // 緑色のカップ
        Yellow  // 黄色のカップ
    }

    // カップの種類をインスペクターで設定できるように公開
    public CombinationManager.CupType cupType; // CombinationManagerのCupTypeを使用
}
