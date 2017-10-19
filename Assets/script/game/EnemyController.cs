using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Tooltip("怪物血量")] [SerializeField] private float healthPoint = 5f;
    [Tooltip("傷害值")] [SerializeField] private float damage = 1f;
    [Tooltip("攻擊音效")] [SerializeField] private AudioClip attackAC;
    [Tooltip("死亡音效")] [SerializeField] private AudioClip deadAC;
    [Tooltip("走路音效")] [SerializeField] private AudioClip walkAC;
    [Tooltip("無敵時間")] [SerializeField] private float delay;

    private UnityEngine.AI.NavMeshAgent navMesh;
    private GameObject player;
    private Animator ani;
    private AudioSource audioSource;
    private string status = "walk";
    private string currentAction = "walk";
    private float _hp;
    private SkinnedMeshRenderer[] meshList;
    private bool invincible;
    void Start()
    {
        _init();
        meshList = this.GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    void PlaySound(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        // audioSource.mute = true;
		audioSource.volume =  3f / Vector3.Distance(this.transform.position,navMesh.destination);
        if (!audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    public void ApplyDamage(float _damageAmount)
    {
        if (!invincible)
        {
            float t = _hp - _damageAmount;
            if (t > 0)
            {
                _hp = t;
                StartCoroutine(EnterInvincible());
            }
            else
            {
                _hp = 0;
                status = "die";
                ActionChange(status);
                HitHapticPulse(1000);
            }
        }
    }
    void ChangeColor(Color input)
    {
        foreach (SkinnedMeshRenderer mesh in meshList)
        {
            mesh.material.color = input;
        }
    }
    void HitHapticPulse(ushort duartion)
    {
        int leftDevice = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        int rightDevice = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        SteamVR_Controller.Input(leftDevice).TriggerHapticPulse(duartion);
        SteamVR_Controller.Input(rightDevice).TriggerHapticPulse(duartion);
    }
    public void SwitchDead()
    {
        ani.SetBool("isDead", true);
        navMesh.enabled = false;

        this.GetComponent<CapsuleCollider>().enabled = true;
        PlaySound(deadAC);
        currentAction = status;

        KillCount killCount = GameObject.FindGameObjectWithTag("UIcount").GetComponent<KillCount>();
        killCount.AddCount(1);
		StartCoroutine(delayRemove(3f));
    }
    void SwitchAttack(bool value)
    {
        ani.SetBool("isAttack", value);
        PlaySound(attackAC);
        currentAction = status;
        if (navMesh.isOnNavMesh)
            navMesh.isStopped = value;
    }

    IEnumerator EnterInvincible()
    {
        invincible = true;
        ChangeColor(Color.red);
        yield return new WaitForSeconds(delay);
        ChangeColor(Color.white);
        invincible = false;
        StopCoroutine(EnterInvincible());
    }
    // Update is called once per frame
    void Update()
    {
        if (navMesh.enabled && _hp > 0)
        {
            navMesh.destination = player.transform.position;
            if (Vector3.Distance(this.transform.position, player.transform.position) < navMesh.stoppingDistance+1)
            {
                status = "atk";
            }
            else
            {
                status = "walk";
            }
            ActionChange(status);
        }
    }
    private float timeCount = 0;
    private float interval = 1f;
    void ActionChange(string nextStatus)
    {
        if (nextStatus.Equals("atk"))
        {
            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
            }
            else
            {
                PlayerScript playerComponent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerScript>();
                playerComponent.ApplyDamage(damage);
                timeCount = interval;
            }
        }

        if (nextStatus.Equals(currentAction)) return;

        if (nextStatus.Equals("walk"))
        {
            SwitchAttack(false);
        }
        if (nextStatus.Equals("atk"))
        {
            SwitchAttack(true);
		}
        if (nextStatus.Equals("die"))
        {
            SwitchDead();
        }
    }
    public void _init()
    {
        navMesh = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMesh == null)
        {
            navMesh = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            navMesh.radius = 0.3f;
        }
        navMesh.enabled = true;

        invincible = false;
        _hp = healthPoint;
        status = "walk";
        currentAction = "walk";

        player = GameObject.FindGameObjectWithTag("Player");
        ani = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }
        PlaySound(walkAC);
    }
	IEnumerator delayRemove(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Lean.LeanPool.Despawn(this.gameObject);
        _init();
    }
}
