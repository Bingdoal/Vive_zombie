using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	[Tooltip("怪物血量")][SerializeField]private float healthPoint = 5f;
	[Tooltip("傷害值")][SerializeField]private float damage = 1f;
	[Tooltip("攻擊音效")][SerializeField]private AudioClip attackAC;
	[Tooltip("死亡音效")][SerializeField]private AudioClip deadAC;
	[Tooltip("走路音效")][SerializeField]private AudioClip walkAC;
	[Tooltip("無敵時間")][SerializeField] private float delay ;
	
	private UnityEngine.AI.NavMeshAgent agent;
	private GameObject player;
	private Animator ani;
	private AudioSource audioSource;
	private string status = "walk";
	private	string currentAction = "walk";
	private float _hp ;
	private SkinnedMeshRenderer[] meshList;
	private bool invincible;
	void OnEnable(){
		agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (agent == null) {
			agent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
			agent.radius = 0.3f;
		}
		agent.enabled = true;
	}
	void Start () {
		_init();
		meshList = this.GetComponentsInChildren<SkinnedMeshRenderer>();
	}
	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="ac">Ac.</param>
	void PlaySound(AudioClip audioClip){
		audioSource.Stop();	
		audioSource.clip = audioClip;
		audioSource.mute = true;
		if(!audioSource.isPlaying){
			audioSource.loop = true;
			audioSource.Play();
		}
	}
	private Color flash ;
	public void ApplyDamage(float _damageAmount){
		if(!invincible){
			float t = _hp - _damageAmount;
			if (t > 0) {
				print(invincible);
				_hp = t;
				StartCoroutine(EnterInvincible());
			} else {
				_hp = 0;
				status = "die";
				ActionChange(status);
				HitHapticPulse (1000);
			}
		}
		

	}
	void ChangeColor(Color input){
		foreach(var mesh in meshList){
			mesh.material.color = input;
			print(input);
		}
	}
	void HitHapticPulse(ushort duartion){
		var deviceIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);
		var deviceIndex1 = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);
		SteamVR_Controller.Input (deviceIndex).TriggerHapticPulse (duartion);
		SteamVR_Controller.Input (deviceIndex1).TriggerHapticPulse (duartion);
	}
	void SwitchDead(){
		ani.SetBool("isDead",true);
		agent.enabled = false;
		this.GetComponent<CapsuleCollider>().enabled = true;
		PlaySound(deadAC);
		currentAction = status;
	}
	void SwitchAttack(bool value){
		ani.SetBool("isAttack",value);
		PlaySound(attackAC);
		currentAction = status;
	}
	
	IEnumerator EnterInvincible(){
		invincible = true;
		ChangeColor(Color.red);
		yield return new WaitForSeconds(delay);
		ChangeColor(Color.white);
		invincible = false;
		StopCoroutine(EnterInvincible());
	}
	// Update is called once per frame
	int i =0 ;
	void Update () {
		if(_hp <=0){
			AnimatorStateInfo asi = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
			if(asi.normalizedTime>1f && asi.IsName("back_fall")){
				_init();
				Lean.LeanPool.Despawn(this.gameObject);
			}
		}
		if(agent.enabled && _hp>0){
			agent.destination = player.transform.position;
			if(Vector3.Distance(this.transform.position,player.transform.position)<3f){
				status = "atk";
			}else{
				status = "walk";
			}
		}
		ActionChange(status);
	}
	private float timeCount = 0;
	private float interval = 1f;
	void ActionChange(string nextStatus){
		if(nextStatus.CompareTo("atk")==0){
			if(timeCount>0){
				timeCount -= Time.deltaTime;
			}else{
				PlayerScript playerComponent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerScript>();
				playerComponent.ApplyDamage(damage);
				timeCount = interval;
			}
		}

		if(nextStatus.CompareTo(currentAction)==0) return;

		if(nextStatus.CompareTo("walk")==0){
			SwitchAttack(false);
			agent.isStopped = false;
			print(status);
		}
		if(nextStatus.CompareTo("atk")==0){
			SwitchAttack(true);
			agent.isStopped = true;
			print(status);
		}
		if(nextStatus.CompareTo("die")==0){
			SwitchDead();
			print(status);
		}
	}
	void _init(){
		invincible = false;
		_hp = healthPoint;
		status = "walk";
		currentAction = "walk";

		player = GameObject.FindGameObjectWithTag ("Player");
		ani = this.GetComponent<Animator> ();
		audioSource = this.GetComponent<AudioSource> ();
		if (audioSource == null) {
			audioSource = this.gameObject.AddComponent<AudioSource> ();
		}
		PlaySound(walkAC);
	}
}
