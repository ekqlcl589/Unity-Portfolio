using System.Collections;
using UnityEngine;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviour 
{
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    private bool isInput = true;

    private bool isNight = false;

    private int zombieCount = Constants.ZOMBIE_START_COUNT;

    private int dayCount = Constants.DAY_START_COUNT;

    private bool last = false;

    private bool clear = false;

    private bool safeHouse = false;

    private int weaponNum = Constants.WEAPONE_NUMBER1;

    // 싱글톤 접근용 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    public System.Action action;

    public bool isGameover 
    { 
        get; 
        private set;
    } // 게임 오버 상태

    public bool IsNight
    {
        get => isNight;

        set
        {
            if (m_instance is null)
                return;

            isNight = value;
        }
    }

    public bool Last
    {
        get => last;
        set
        {
            if (m_instance is null)
                return;

            last = value;
        }
    }

    public bool Clear
    {
        get => clear;
        set
        {
            if (m_instance is null)
                return;

            clear = value;
        }
    }

    public bool SafeHouse
    {
        get => safeHouse;
        set
        {
            if (m_instance is null)
                return;

            safeHouse = value;
        }
    }

    public int WeaponNum
    {
        get => weaponNum;
        set
        {
            if (m_instance is null)
                return;

            weaponNum = value;
        }
    }
    public bool IsInput
    {
        get => isInput;
        set
        {
            if (m_instance is null)
                return;

            isInput = value;
        }
    }

    public int ZombieCount
    {
        get => zombieCount;
        set
        {
            if (m_instance is null)
                return;

            zombieCount = value;
        }
    }
    void Awake() {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (Instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        //action += AddDayPoint;    

        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;

    }

    public void AddDayCount(int newDayCount)
    {
        if(!isGameover)
        {
            dayCount += newDayCount;

            UIManager.Instance.UpdateAliveDayText(dayCount);
            UIManager.Instance.UpdateAliveText(dayCount);

            //AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.day3,
            //    day: dayCount);
            //
            //AchievementsManager.Instance.OnNotify(AchievementsManager.Achievements.day7,
            //    day: dayCount);

            AchievementsManager.Achievements combinedAchievements = AchievementsManager.Achievements.day3 | AchievementsManager.Achievements.day7;
            AchievementsManager.Instance.OnNotify(combinedAchievements, value: dayCount);

        }
    }
    public void AddZombieCount(int newCount)
    {
        if (!isGameover)
        {
            zombieCount += newCount;

            UIManager.Instance.UpdateDieReasonText(zombieCount);
            UIManager.Instance.UpdateKillText(zombieCount);
        }
    }
    // 게임 오버 처리
    public void EndGame() {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.Instance.SetActiveGameoverUI(true);

    }
    public void ReGame()
    {
        Destroy(gameObject);
        isGameover = false;
        UIManager.Instance.SetActiveGameoverUI(false);
    }
}