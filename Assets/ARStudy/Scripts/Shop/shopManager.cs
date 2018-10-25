using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

	public static ShopManager instance = null;
	//GUN 
	public int gun_price=0;
	public int potion_price=0;
	public string gun_name=null;
	public string potion_name=null;
	public int gun_damage=0;
	public string gun_explain;
	public string potion_explain;
	public Sprite gun_image;
	public Sprite potion_image;
	public int potion_heal;

	public int gunBuy_check = 0;
	public int potionBuy_check = 0;
	//UI
	public Text select_price;
	public Text select_name;
	public Text select_damage;
	public Text select_heal;
	public Text select_explain;
	public Image select_image;
	public GameObject preview_Camvas;
	public Text kor;
	public GameObject Gun_menu;
	public GameObject Potoin_menu;

	public GameObject block;
	// Use this for initialization
	public static ShopManager Instance{
		get{
			return instance;
		}
	}
	void Awake(){
		if(instance!=null){
			DestroyImmediate(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	