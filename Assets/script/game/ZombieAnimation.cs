using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animation))]
public class ZombieAnimation : MonoBehaviour {
	[Tooltip("怪物血量")][SerializeField] private float healthPoint = 5f;
	[Tooltip("傷害值")][SerializeField] private float damage = 1f;
	[Tooltip("攻擊音效")][SerializeField] private AudioClip attackAC;
	[Tooltip("死亡音效")][SerializeField] private AudioClip deadAC;
	[Tooltip("走路音效")][SerializeField] private AudioClip walkAC;

	private NavMeshAgent agent;
	private GameObject player;
	private Animation ani;
	private float duration = 2.2f;
	private float curTime = 0;
	void OnEnable(){
		agent = this.GetComponent<NavMeshAgent>();
		if(agent==null){
			agent = gameObject.AddComponent<NavMeshAgent>();
			agent.radius = 0.3f;
		}
		agent.enabled = true;
	}

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		ani = this.GetComponent<Animation>();
		ani.wrapMode = WrapMode.Loop;
		// ani["back_fall"].wrapMode = WrapMode.Once;
		print(player.name);
		print(ani.name);
	}

	void Update(){
		// Zombie Dead
		if(healthPoint<=0){
			curTime += Time.deltaTime;
			if(curTime > duration){
				curTime = 0;
				Lean.LeanPool.Despawn(this.gameObject);
			}
		}
		// Zombie alive
		if(agent.enabled && healthPoint>0){
			agent.destination = player.transform.position;
			float dis = Vector3.Distance(this.transform.position, player.transform.position);
			if(dis <2.5f){
				SwitchAttack();
				agent.Stop();
			}else{
				SwitchWalk();
				agent.Resume();
			}
		}
	}

	void PlaySound(AudioClip audioClip){
		AudioSource audioSource = this.GetComponent<AudioSource>();
		if(audioSource == null) 
			audioSource = this.gameObject.AddComponent<AudioSource>();
		audioSource.clip = audioClip;
		audioSource.Play();
	}

	void ApplyDamage(float _damageAmount){
		float tmpHp = healthPoint - _damageAmount;
		if(tmpHp > 0){
			healthPoint = tmpHp;
		}else{
			healthPoint = 0;
			SwitchDead();
			HitHapticPulse(2000);
		}
	}

	void HitHapticPulse(ushort duration){
		int leftIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
		int rightIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
		SteamVR_Controller.Input(leftIndex).TriggerHapticPulse(duration);
		SteamVR_Controller.Input(rightIndex).TriggerHapticPulse(duration);
	}
	void SwitchAttack(){
		// ani.CrossFade("attack");
		PlaySound(attackAC);
	}
	void SwitchWalk(){
		// ani.CrossFade("walk");
		PlaySound(walkAC);
	}
	void SwitchDead(){
		// ani.CrossFade("back_fall");
		this.GetComponent<CapsuleCollider>().enabled = false;
		agent.enabled = false;
		PlaySound(deadAC);
	}	

}
