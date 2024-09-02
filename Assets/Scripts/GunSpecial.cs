using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable�� gunData�� �޾Ƽ� �� ���� 
public class GunSpecial : MonoBehaviour
{
    [SerializeField]
    private Transform fireTransform; // ź���� �߻�� ��ġ

    [SerializeField]
    private ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��

    [SerializeField]
    private ParticleSystem shellEjectEffect; // ź�� ���� ȿ��

    [SerializeField]
    private GunData gunData; // �ѱ� ������ ���̺�

    private LineRenderer bulletLineRenderer; // ź�� ������ �׸��� ���� ������

    private AudioSource gunAudioPlayer; // �� �Ҹ� �����

    private float fireDistance = Constants.ATTACK_DEFAULT_DISTANCE; // �����Ÿ�

    public int ammoRemain = Constants.ATTACK_DEFAULT_AMMO; // ���� ��ü ź��
    public int magAmmo; // ���� ź������ ���� �ִ� ź��

    private float lastFireTime; // ���� ���������� �߻��� ����

    public enum State
    {
        Ready, // �߻� �غ��
        Empty, // ź������ ��
        Reloading // ������ ��
    }

    public State state { get; private set; } // ���� ���� ����

    private void Awake()
    {
        // ����� ������Ʈ�� ���� ��������
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = Constants.LINERENDERER_POSITIONCOUNT2;

        bulletLineRenderer.enabled = false;

        gunData.gun = GunData.gunType.gunSpecial;
    }

    private void OnEnable()
    {
        // �� ���� �ʱ�ȭ
        ammoRemain = gunData.startAmmoRemain;

        magAmmo = gunData.magCapacity;

        state = State.Ready;

        lastFireTime = 0;
    }

    // �߻� �õ�, ���� �߻� ������ ���¿����� Shot() �Լ��� �����Ű���� ���δ� ����
    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire) // ������ �� �߻� �������� �ǵ�����.Ÿ�Ӻ����̾� �̻��� �ð��� ������ �� == ���� �ð��� ���� �ֱٿ� �߻��� ���� + �߻� ���� ���� ���� 
        {
            // ������ �� �߻� ���� ����
            lastFireTime = Time.time;

            Shot();
        }
    }

    // ���� �߻� ó��
    private void Shot()
    {
        // ����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� �����̳�
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        // ����ĳ��Ʈ(���� ����, ����, �浹 ���� �����̳�, �����Ÿ�
        if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            // ���̰� � ��ü�� �浹�� ���

            // �浹�� �������� ���� IDamageable ������Ʈ �������� �õ�
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                // ������ OnDamage �Լ��� ������� ���濡�� ������ ó��
                target.OnDamage(Constants.ATTACK_DEFAULT_DAMAGE, hit.point, hit.normal);
            }

            // ���̰� �浹�� ��ġ ����
            hitPosition = hit.point;

        }
        else
        {
            // ���̰� �浹���� �ʾ�����, ź���� �ִ�����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
            hitPosition = fireTransform.position + fireTransform.forward * Constants.ATTACK_DEFAULT_DISTANCE;
        }

        // �߻� ����Ʈ ���
        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;

        if (magAmmo <= Constants.DEFAULT_NUMBER_0)
        {
            state = State.Empty;
        }
    }

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� ź�� ������ �׸�
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // �ѱ� ȭ�� ȿ�� ���
        muzzleFlashEffect.Play();
        // ź�� ���� ȿ�� ���
        shellEjectEffect.Play();
        //�Ѱ� �Ҹ� ���
        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        //���� �������� �ѱ��� ��ġ
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        //���� ������ �Է����� ���� �浹 ��ġ
        bulletLineRenderer.SetPosition(1, hitPosition);

        // ���� �������� Ȱ��ȭ�Ͽ� ź�� ������ �׸�
        bulletLineRenderer.enabled = true;

        // 0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);
        //yield return new WaitForSeconds(1f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� ź�� ������ ����
        bulletLineRenderer.enabled = false;
    }

    // ������ �õ�
    public bool Reload()
    {

        if (state == State.Reloading || ammoRemain <= Constants.DEFAULT_NUMBER_0 || magAmmo >= gunData.magCapacity)
        {
            return false;
        }

        // ������ ó�� ����
        StartCoroutine(ReloadRoutine());
        return true;
    }

    // ���� ������ ó���� ����
    private IEnumerator ReloadRoutine()
    {
        // ���� ���¸� ������ �� ���·� ��ȯ
        state = State.Reloading;

        gunAudioPlayer.PlayOneShot(gunData.reloadClip);

        // ������ �ҿ� �ð� ��ŭ ó�� ����
        yield return new WaitForSeconds(gunData.reloadTime);

        // źâ�� ä�� ź�� ���
        int ammoToFill = gunData.magCapacity - magAmmo;

        if (ammoRemain < ammoToFill)
            ammoToFill = ammoRemain;

        magAmmo += ammoToFill;

        ammoRemain -= ammoToFill;

        // ���� ���� ���¸� �߻� �غ�� ���·� ����
        state = State.Ready;
    }
}
