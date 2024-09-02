using UnityEngine;

// 좀비 생성시 사용할 셋업 데이터 Scriptable로 에디터 상에서 값을 쉽게 변경할 수 있게 만듦 
[CreateAssetMenu(menuName = "Scriptable/ZombieData", fileName = "Zombie Data")]
public class ZombieData : ScriptableObject {
    public float health = 100f; // 체력
    public float damage = 20f; // 공격력
    public float speed = 2f; // 이동 속도
    public Color skinColor = Color.white; // 피부색
}
