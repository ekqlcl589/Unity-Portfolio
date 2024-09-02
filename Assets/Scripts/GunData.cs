using UnityEngine;

// Scriptable로 에디터 상에서 값을 쉽게 변경할 수 있게 만듦 
[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "Gun Data")]
public class GunData : ScriptableObject
{
    public enum gunType
    {
        gunNormal,
        gunSpecial,
    }

    public gunType gun;
    public AudioClip shotClip; // 발사 소리
    public AudioClip reloadClip; // 재장전 소리

    // 디폴트 값 에디터에서 즉시 변경 가능함
    public float damage = 25; 

    public int startAmmoRemain = 100; 
    public int magCapacity = 25;

    public float timeBetFire = 0.12f; 
    public float reloadTime = 1.8f;

}