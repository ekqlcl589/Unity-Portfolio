using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity 
{
    [SerializeField]
    private Slider healthSlider; // 체력을 표시할 UI 슬라이더

    [SerializeField]
    private Slider hungerSlider;

    [SerializeField]
    private Slider temperatureSlider;

    [SerializeField]
    private AudioClip deathClip; // 사망 소리

    [SerializeField]
    private AudioClip hitClip; // 피격 소리

    [SerializeField]
    private AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    private bool isGod = false;

    private void Awake() {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();

    }

    public void Start()
    {
        currentPlayerState = BitFlag.StateFlags.none;

        currentHungryDecreaseTime = Time.deltaTime;
        hungryDecreaseTime = hungryDecreasePoint * Time.deltaTime;

        currentTemperatureDecreaseTime = Time.deltaTime;
        temperatureDecreaseTime = temperatureDecreasePoint * Time.deltaTime;

        StartCoroutine(UpdateHungerGauge());
        StartCoroutine(UpdateTemperatureGauge());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
                isGod = true;
        }
    }
    protected override void OnEnable() {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);

        healthSlider.maxValue = startingHealth;

        healthSlider.value = health;

        hungerSlider.gameObject.SetActive(true);

        hungerSlider.maxValue = maxHunger;

        hungerSlider.value = Hunger;

        temperatureSlider.gameObject.SetActive(true);

        temperatureSlider.maxValue = maxTemperature;

        temperatureSlider.value = Temperature;

        playerMovement.enabled = true;

        playerShooter.enabled = true;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth) {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }


    public override void RestoreHunger(float newHunger)
    {
        base.RestoreHunger(newHunger);

        hungerSlider.value = Hunger;
    }

    public override void Diminish(float newHunger)
    {
        base.Diminish(newHunger);

        hungerSlider.value = Hunger;
    }

    public override void RestoreTemperature(float newTemper)
    {
        base.RestoreTemperature(newTemper);

        temperatureSlider.value = Temperature;
    }

    public override void DownTemperature(float newTemper)
    {
        base.DownTemperature(newTemper);

        temperatureSlider.value = Temperature;
    }
    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection) {

        if(dead is false)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        if(isGod is false)
            base.OnDamage(damage, hitPoint, hitDirection);

        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die() 실행(사망 적용)
        if(isGod is false)
            base.Die();

        healthSlider.gameObject.SetActive(false);

        playerAudioPlayer.PlayOneShot(deathClip);

        playerAnimator.SetBool("Dead", true);

        playerMovement.enabled = false;

        playerShooter.enabled = false;

        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other) {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리

        if(!dead)
        {
            IItem item = other.GetComponent<IItem>();
        
            if(item != null)
            {
               item.Use(gameObject);
        
               // playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }

    private IEnumerator UpdateHungerGauge()
    {
        while(!dead && !GameManager.Instance.SafeHouse && !isGod)
        {
            if (Hunger >= maxHunger)
            {
                currentPlayerState &= ~BitFlag.StateFlags.hungry;
                Debug.Log("Current State: " + currentPlayerState);
            }

            if (Hunger >= 0)
            {
                if (currentHungryDecreaseTime <= hungryDecreaseTime)
                    currentHungryDecreaseTime++;
                else
                {
                    Hunger -= hungryDecreasePoint;
                    currentHungryDecreaseTime = Time.time;
                    hungerSlider.value = Hunger;
                }
            }
            else // 배고픔 게이지가 0보다 작아지면 체력이 떨어지면서 배고픔 상태 추가
            {
                currentPlayerState |= BitFlag.StateFlags.hungry; // hungry 플래그 추가
                Debug.Log("Current State: " + currentPlayerState);

                health -= hungryDecreasePoint;
                healthSlider.value = health;

                if(health <= 0)
                {
                    Die();
                    break;
                }
                yield return new WaitForSeconds(1f); // 테스트용 나중에는 시간값 더 올려서 사용

            }
            yield return new WaitForSeconds(5f); // 테스트용 나중에는 시간값 더 올려서 사용
        }
    }

    private IEnumerator UpdateTemperatureGauge()
    {
        while(!dead && !GameManager.Instance.SafeHouse && !isGod)
        {
            if (Temperature >= maxTemperature)
            {
                currentPlayerState &= ~BitFlag.StateFlags.cold;
                Debug.Log("Current State: " + currentPlayerState);
            }

            if (Temperature >= 0)
            {
                if (currentTemperatureDecreaseTime <= temperatureDecreaseTime)
                    currentTemperatureDecreaseTime++;
                else
                {
                    Temperature -= temperatureDecreasePoint;
                    currentTemperatureDecreaseTime = Time.time;
                    temperatureSlider.value = Temperature;
                }
            }
            else // 온도 게이지가 0보다 작아지면 체력이 떨어지면서 상태 추가 
            {
                currentPlayerState |= BitFlag.StateFlags.cold;
                Debug.Log("Current State: " + currentPlayerState);

                health -= temperatureDecreasePoint;
                healthSlider.value = health;

                if(health <= 0)
                {
                    Die();
                    break;
                }
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(7f);
        }
    }
}