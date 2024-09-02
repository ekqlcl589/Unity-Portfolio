using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {
    // 플레이어 관련
    public const float ZERO_FORCE = 0f;
    public const float MOVE_SPEED = 5f;
    public const float ROATATE_SPEED = 180f;
    public const float ROTATE_FORCE = 90f;
    public const float JUMP_FORCE = 700f;

    public const float PLAYER_DIE = 0f;
    public const float PLAYER_MAX_HEALTH = 100f;
    public const float PLAYER_MAX_HUNGER = 100f;
    public const float PLAYER_MAX_TEMPERATURE = 100f;
    public const float PLAYER_HUNGRYDECREASEPOINT = 5f;
    public const float PLAYER_TEMPERATUREDECREASEPOINT = 5f;
    public static float PLAYER_START_HEALTH = 100f;
    public static float PLAYER_START_HUNGER = 100f;
    public static float PLAYER_START_TEMPERATURE = 100f;

    public const int LINERENDERER_POSITIONCOUNT2 = 2;
    public static float ATTACK_DEFAULT_DISTANCE = 50f;
    public static float ATTACK_DEFAULT_DAMAGE = 25f;
    public static int ATTACK_DEFAULT_AMMO = 100;
    public static int ATTACK_MAGCAPACITY = 25;
    public static float ATTACK_TIMEBET = 0.12f;
    public static float ATTACK_RELOAD_TIME = 1.8f;

    // 게임 매니저 관리 상수
    public static int ZOMBIE_START_COUNT = 0;
    public static int DAY_START_COUNT = 0;
    public static int WEAPONE_NUMBER1 = 0;
    public static int WEAPONE_NUMBER2 = 1;

    // 좀비 및 동물 이동 관련
    public const float ZOMBIE_DEFAULT_HEALTH = 100f;
    public const float ZOMVBIE_DIE = 0f;
    public const float ZOMBIE_DEFAULT_DAMAGE = 20f;
    public const float ZOMBIE_DEFAULT_SPEED = 2f;
    public const float ZOMBIE_DOWN_SPEED = 0.5f;
    public const float ZOMBIE_START_DISSOLBE = 1f;
    public const float ZOMBIE_DEST_DISSOLBE = 0f;
    public const float BOSS_ZOMBIE_DEFAULT_DAMAGE = 50f;
    public const float BOSS_ZOMBIE_HALF_HEALTH = 500f;

    public const float ZOMBIE_DEFAULT_DISSOLVE_TIME = 2f;

    public const float BEAR_DEFAULT_SPEED = 1f;
    public const float BEAR_DEFAULT_HEALTH = 200f;
    public const float BEAR_DEFAULT_DAMAGE = 25f;

    public const float PIG_DEFAULT_SPEED = 1f;
    public const float PIG_DEFAULT_HEALTH = 20f;

    public const float CHICKEN_DEFAULT_SPEED = 1f;
    public const float CHICKEN_DEFAULT_HEALTH = 20f;

    public const float TIME_BET_ATTACK = 1f;
    public const float SPHERE_REDIUS_5 = 5f;
    public const float SPHERE_REDIUS_10 = 10f;

    public static int ANIMAL_START_COUNT = 0;
    public const float DELETE_TIME = 5f;

    // 기타 상수
    public const int DEFAULT_ADD20 = 20;
    public const int DEFAULT_ADD30 = 30;
    public const int DEFAULT_ADD50 = 50;
    public const float HEALING_POINT = 7f;
    public const float TIMEBET_HEAL = 1f;
    public const float DISTANCE50 = 50f;
    public const float SPAWN_MAX_TIME = 7f;
    public const float SPAWN_MIN_TIME = 2f;
    public const float COOK_MAX_GAUGE = 100f;
    public static float COOK_START_HAUGE = 0f;

    // UI or 기본적인 정수형 데이터 관련 
    public const int MAX_INVENTORY_SLOT = 16;
    public static int DEFAULT_INVENTORY_SLOT = 8;
    public static int DEFAULT_NUMBER_0 = 0;
    public static int DEFAULT_NUMBER_1 = 1;
    public static int DEFAULT_NUMBER_2 = 2;
    public static int DEFAULT_NUMBER_3 = 3;
    public static int DEFAULT_NUMBER_4 = 4;
    public static int DEFAULT_NUMBER_5 = 5;
    public static int DEFAULT_NUMBER_6 = 6;
    public static int DEFAULT_NUMBER_7 = 7;
    public static int DEFAULT_NUMBER_8 = 8;
    public static int DEFAULT_NUMBER_9 = 9;
    public static int DEFAULT_NUMBER_10 = 10;

    // 빌드 인덱스 및 플레이어 포지션 설정 관련
    public const int BUILD_INDEX1 = 1;
    public const int BUILD_INDEX2 = 2;
    public const int BUILD_INDEX3 = 3;
    public const int BUILD_INDEX4 = 4;
    public const float NEW_POSITION_X = 0f;
    public const float NEW_POSITION_Y = 4f;
    public const float NEW_POSITION_Z = 0f;

    // 셰이더 적용 관련
    public const float PHASE_TIME = 2f;
    public const int PHASE_START_TIME = 0;
    public const int PHASE_END_TIME = 17;
}
