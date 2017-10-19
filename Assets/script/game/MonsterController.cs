using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{


    [Tooltip("怪物血量")] [SerializeField] private float healthPoint = 5f;
    [Tooltip("傷害值")] [SerializeField] private float damage = 1f;
    [Tooltip("攻擊音效")] [SerializeField] private AudioClip attackAC;
    [Tooltip("死亡音效")] [SerializeField] private AudioClip deadAC;
    [Tooltip("走路音效")] [SerializeField] private AudioClip walkAC;
    [Tooltip("無敵時間")] [SerializeField] private float invincibleDelay;

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
    }
    // Update is called once per frame
    void Update()
    {
        if (navMesh.enabled && _hp > 0)
        {
            navMesh.destination = player.transform.position;
            if (Vector3.Distance(this.transform.position, player.transform.position) < navMesh.stoppingDistance + 1)
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
        if (nextStatus.Equals("atk") && !ani.GetCurrentAnimatorStateInfo(0).IsName("creature1GetHit"))
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
            SwitchGetHit(false);
            SwitchWalk(true);
        }
        if (nextStatus.Equals("atk"))
        {
            SwitchWalk(false);
            SwitchGetHit(false);
            SwitchAttack(true);
        }
        if (nextStatus.Equals("die"))
        {
            SwitchDead();
        }
        if (nextStatus.Equals("GetHit"))
        {
            SwitchWalk(false);
            SwitchAttack(false);
            SwitchGetHit(true);
        }
    }
    // Switch Action
    void SwitchDead()
    {
        ani.SetBool("isDead", true);
        navMesh.enabled = false;

        this.GetComponent<CapsuleCollider>().enabled = true;
        _playSound(deadAC);
        currentAction = status;

        KillCount killCount = GameObject.FindGameObjectWithTag("UIcount").GetComponent<KillCount>();
        killCount.AddCount(1);
        StartCoroutine(_delayRemove(3f));
    }
    void SwitchWalk(bool value)
    {
        if (navMesh.isOnNavMesh)
            navMesh.isStopped = !value;
        if (value)
            _playSound(walkAC);
        currentAction = status;
    }
    void SwitchAttack(bool value)
    {
        ani.SetBool("isAttack", value);
        if (value)
        {
            _playSound(attackAC);
            timeCount = interval;
        }
        currentAction = status;
    }
    void SwitchGetHit(bool isGetHit)
    {
        currentAction = status;
        ani.SetBool("GetHit", isGetHit);
    }

    // Private method
    void _init()
    {
        ani = this.GetComponent<Animator>();

        navMesh = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMesh == null)
        {
            navMesh = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            navMesh.radius = 0.3f;
        }
        if (navMesh.isOnNavMesh)
            navMesh.isStopped = true;
        invincible = false;
        _hp = healthPoint;

        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = this.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }
        meshList = this.GetComponentsInChildren<SkinnedMeshRenderer>();

        StartCoroutine(_animationDelay());

    }
    void _startWalk()
    {
        status = "walk";
        currentAction = "walk";
        navMesh.enabled = true;
        SwitchAttack(false);
        SwitchGetHit(false);
        SwitchWalk(true);
    }
    void _playSound(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        // audioSource.mute = true;
        audioSource.volume = 3f / Vector3.Distance(this.transform.position, navMesh.destination);
        if (!audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    void _changeColor(Color input)
    {
        foreach (SkinnedMeshRenderer mesh in meshList)
        {
            mesh.material.color = input;
        }
    }
    void _hitHapticPulse(ushort duartion)
    {
        int leftDevice = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        int rightDevice = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        SteamVR_Controller.Input(leftDevice).TriggerHapticPulse(duartion);
        SteamVR_Controller.Input(rightDevice).TriggerHapticPulse(duartion);
    }
    private IEnumerator _animationDelay()
    {
        invincible = true;
        yield return new WaitForSeconds(3f);
        ani.SetBool("isSpawning", false);
        invincible = false;
        _startWalk();
        StopCoroutine(_animationDelay());
    }
    IEnumerator _enterInvincible()
    {
        invincible = true;
        _changeColor(Color.red);
        yield return new WaitForSeconds(invincibleDelay);
        _changeColor(Color.white);
        invincible = false;
        StopCoroutine(_enterInvincible());
    }
    IEnumerator _delayRemove(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Lean.LeanPool.Despawn(this.gameObject);
        _init();
    }

    // Public method
    public void ApplyDamage(float _damageAmount)
    {
        if (!invincible)
        {
            float t = _hp - _damageAmount;
            if (t > 0)
            {
                _hp = t;
                StartCoroutine(_enterInvincible());
                status = "GetHit";
                ActionChange(status);
            }
            else
            {
                _hp = 0;
                status = "die";
                ActionChange(status);
                _hitHapticPulse(1000);
            }
        }
    }
}