using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    [Header("Reference Objects")]
    public GameManager gameManager;
    public Image[] bulletIcons;

    [Header("Weapon Settings")]
    public int maxAmmo = 7;
    private int currentAmmo;
    public float jamChance = 0.1f;
    private bool isJammed = false;
    private bool isReloading = false;
    private bool isFixingJam = false;

    [Header("Animator")]
    public Animator gunAnimator;

    [Header("Audio Clips")]
    public AudioClip shootClip;
    public AudioClip jamClip;
    public AudioClip reloadClip;
    public AudioClip fixClip;

    private AudioSource _audioSource;
    private static readonly int ShootHash = Animator.StringToHash("ShootTrigger");
    private static readonly int JamHash = Animator.StringToHash("JamTrigger");
    private static readonly int ReloadHash = Animator.StringToHash("ReloadTrigger");
    private static readonly int FixHash = Animator.StringToHash("FixGunTrigger");

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;
        if (Cursor.visible)
            Cursor.visible = false;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Translate(new Vector3(mouseX, mouseY, 0));

        if (Input.GetMouseButtonDown(0))
            HandleShooting();
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            StartCoroutine(RunReload());
        if (Input.GetKeyDown(KeyCode.F) && isJammed && !isFixingJam)
            StartCoroutine(RunFixJam());
    }

    void HandleShooting()
    {
        if (isJammed || isReloading || isFixingJam) return;
        if (currentAmmo <= 0) return;

        gameManager.totalShots += 1;
        gunAnimator.SetTrigger(ShootHash);
        if (shootClip != null) _audioSource.PlayOneShot(shootClip);

        if (Random.value < jamChance)
        {
            isJammed = true;
            gunAnimator.SetTrigger(JamHash);
            if (jamClip != null) _audioSource.PlayOneShot(jamClip);
            return;
        }

        currentAmmo--;
        UpdateAmmoUI();
        CheckHit();
    }

    void CheckHit()
    {
        Vector2 center = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        Collider2D hit = Physics2D.OverlapPoint(center);
        if (hit != null)
        {
            if (hit.GetComponent<StaticTarget>() != null) gameManager.AddScore(10);
            else if (hit.GetComponent<MovingTarget>() != null) gameManager.AddScore(20);
            else if (hit.GetComponent<IrregularTarget>() != null) gameManager.AddScore(50);
            Destroy(hit.gameObject);
        }
    }

    IEnumerator RunReload()
    {
        isReloading = true;
        gunAnimator.SetTrigger(ReloadHash);
        if (reloadClip != null) _audioSource.PlayOneShot(reloadClip);
        yield return StartCoroutine(WaitForAnimationEnd("Reload"));
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        isReloading = false;
    }

    IEnumerator RunFixJam()
    {
        isFixingJam = true;
        gunAnimator.SetTrigger(FixHash);
        if (fixClip != null) _audioSource.PlayOneShot(fixClip);
        yield return StartCoroutine(WaitForAnimationEnd("FixGun"));
        isJammed = false;
        isFixingJam = false;
    }

    IEnumerator WaitForAnimationEnd(string stateName)
    {
        while (!gunAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;
        var info = gunAnimator.GetCurrentAnimatorStateInfo(0);
        while (info.normalizedTime < 1f)
        {
            yield return null;
            info = gunAnimator.GetCurrentAnimatorStateInfo(0);
        }
    }

    void UpdateAmmoUI()
    {
        for (int i = 0; i < bulletIcons.Length; i++)
            bulletIcons[i].enabled = (i < currentAmmo);
    }
}
