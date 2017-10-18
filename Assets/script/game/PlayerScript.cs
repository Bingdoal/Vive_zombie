using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    [Tooltip("血量")] [SerializeField] private float healthPoint;
    [Tooltip("被打音效")] [SerializeField] private AudioClip attackedAC;
    [Tooltip("死亡音效")] [SerializeField] private AudioClip dieAC;
    private AudioSource audioSource;
    private float _hp;
    public string status = "alive";
    // Use this for initialization
    void Start()
    {
        _hp = healthPoint;
        _changeUI(_hp);
        audioSource = this.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ApplyDamage(float _damage)
    {
        float t = _hp - _damage;
        PlaySound(attackedAC);
        if (t > 0)
        {
            _hp = t;
        }
        else
        {
            status = "dead";
            _hp = healthPoint;
            _playerDead();
        }
        _changeUI(_hp);
    }
    void PlaySound(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

    }
    void _changeUI(float hpOnUI)
    {
        UIScript uIScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UIScript>();
        uIScript.setHP(hpOnUI);
    }
    void _playerDead()
    {
        StartGame menu = GameObject.FindGameObjectWithTag("menu").GetComponent<StartGame>();
        menu.stopGame();
        PlaySound(dieAC);
    }
}
