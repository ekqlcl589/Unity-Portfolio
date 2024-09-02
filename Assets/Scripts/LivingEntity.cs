using System;
using UnityEngine;

// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class LivingEntity : MonoBehaviour, IDamageable {
    protected BitFlag.StateFlags currentPlayerState;

    protected float maxHealth = Constants.PLAYER_MAX_HEALTH;
    protected float startingHealth = Constants.PLAYER_START_HEALTH; // 시작 체력

    protected float maxHunger = Constants.PLAYER_MAX_HUNGER;
    protected float startingHunger = Constants.PLAYER_START_HUNGER;

    protected float hungryDecreasePoint = Constants.PLAYER_HUNGRYDECREASEPOINT;
    protected float hungryDecreaseTime;
    protected float currentHungryDecreaseTime;

    protected float maxTemperature = Constants.PLAYER_MAX_TEMPERATURE;
    protected float startingTemperature = Constants.PLAYER_START_TEMPERATURE;

    protected float temperatureDecreasePoint = Constants.PLAYER_TEMPERATUREDECREASEPOINT;
    protected float temperatureDecreaseTime;
    protected float currentTemperatureDecreaseTime;

    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable() {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        health = startingHealth;
        Hunger = startingHunger;
        Temperature = startingTemperature;
    }

    public float health { get; protected set; } // 현재 체력
    public float Temperature { get; protected set; }
    public float Hunger { get; protected set; }

    public float MaxTemperature { get => maxTemperature; }

    public bool dead { get; protected set; } // 사망 상태

    public event Action onDeath; // 사망시 발동할 이벤트

    // 데미지를 입는 기능, IDamageable 상속 
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        // 충돌 시 데미지, 충돌 위치, 방향
        health -= damage;

        // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (health <= Constants.PLAYER_DIE && !dead)
        {
            GameManager.Instance.AddZombieCount(1);

            Die();
        }
    }

    // 체력을 회복하는 기능
    public virtual void RestoreHealth(float newHealth) {
        if (dead is true)
        {
            // 이미 사망한 경우 체력을 회복할 수 없음
            return;
        }

        if (health >= Constants.PLAYER_MAX_HEALTH)
            return;
        else
             health += newHealth;
        // 체력 추가
    }

    public virtual void RestoreHunger(float newHunger)
    {
        if (dead)
            return;

        if (Hunger >= Constants.PLAYER_MAX_HUNGER)
            return;
        else
            Hunger += newHunger;
    }

    public virtual void Diminish(float newHunger)
    {
        if (dead)
            return;

        if (Hunger <= Constants.PLAYER_DIE)
            return;
        else
            Hunger -= newHunger;
    }

    public virtual void RestoreTemperature(float newTemper)
    {
        if (dead)
            return;

        if (Temperature >= Constants.PLAYER_MAX_TEMPERATURE)
            return;
        else
            Temperature += newTemper;
    }

    public virtual void DownTemperature(float newTemper)
    {
        if (dead)
            return;

        if (Hunger <= Constants.PLAYER_MAX_HUNGER)
            return;
        Temperature -= newTemper;

    }
    // 사망 처리
    public virtual void Die() {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (onDeath != null)
        {
            onDeath();
        }

        // 사망 상태를 참으로 변경
        dead = true;
    }
}