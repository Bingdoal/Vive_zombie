using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZEffects;
public class gun_script : MonoBehaviour
{
    public GameObject controllerLeft;
    [Tooltip("發射延遲")] [SerializeField] private float delay = 0.2f;
    [Tooltip("槍聲")] [SerializeField] private AudioClip gun_sound;
    [Tooltip("子彈數量")] [SerializeField] private int bullet = 30;
    [Tooltip("填彈音效")] [SerializeField] private AudioClip reloadsound;
    [Tooltip("缺彈音效")] [SerializeField] private AudioClip bulletEmpty;


    private GameObject bulletRemain;
    private AudioSource audioSource;
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;
    private SteamVR_TrackedController controller;
    public EffectTracer tracerEffect;
    public Transform muzzleTransform;
    private int bulletNum;
    // Use this for initialization
    void Start()
    {
        bulletNum = bullet;
        _bulletDisplay();
        controller = controllerLeft.GetComponent<SteamVR_TrackedController>();
        trackedObject = controllerLeft.GetComponent<SteamVR_TrackedObject>();

        audioSource = this.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }
    }
    private void TriggerPressed(object sender, ClickedEventArgs e)
    {
        ShootWeapon();
    }
    void ShootWeapon()
    {

        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(muzzleTransform.position, muzzleTransform.forward);
        device = SteamVR_Controller.Input((int)trackedObject.index);
        _playSound(gun_sound);
        device.TriggerHapticPulse(3999);
        tracerEffect.ShowTracerEffect(muzzleTransform.position, muzzleTransform.forward, 250f);
        bulletNum--;
        if (Physics.Raycast(ray, out hit, 5000f))
        {
            if (hit.collider.attachedRigidbody)
            {
                if (hit.collider.gameObject.tag.Equals("monster"))
                {
                    try{
                        EnemyController collisionObject = hit.collider.gameObject.GetComponent<EnemyController>();
                        collisionObject.ApplyDamage(2);
                    }catch{
                        MonsterController collisionObject = hit.collider.gameObject.GetComponent<MonsterController>();
                        collisionObject.ApplyDamage(2);
                    }
                }
                else
                {
                    if (hit.collider.gameObject.name.Contains("startbutton"))
                    {
                        EnemySpawnController enemySpawnController = GameObject.FindGameObjectWithTag("SpawnPoints").GetComponent<EnemySpawnController>();
                        enemySpawnController.SetGameStatus("playing");
                        StartGame menu = GameObject.FindGameObjectWithTag("menu").GetComponent<StartGame>();
                        menu.GameStart();
                    }
                }

            }
        }

    }
    
    // Update is called once per frame
    private float timeCount = 0;
    void Update()
    {
        if (controller.triggerPressed && bulletNum > 0)
        {

            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
            }
            else
            {
                ShootWeapon();
                timeCount = delay;
            }

        }
        else if (bulletNum <= 0 && controller.triggerPressed)
        {
            _noBullet();
        }
        _bulletDisplay();
    }
    private void _bulletDisplay()
    {
        UIScript uIScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UIScript>();
        uIScript.setBullet(bulletNum);
    }
    private void _noBullet()
    {
        if (!audioSource.isPlaying)
        {
            _playSound(bulletEmpty);
        }
    }
    void _playSound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    // Public method
    public void Reloadbullet()
    {
        _playSound(reloadsound);
        bulletNum = bullet;
        _bulletDisplay();
    }
}
