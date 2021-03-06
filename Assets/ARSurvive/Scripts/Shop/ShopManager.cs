﻿using System.Collections;
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
	public Text point;
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

	void Start(){
		this.UpdatePoint();
		SoundManager.I.ChangeBGM("Shop_BGM");
	}

	public void UpdatePoint(){
		point.text = PlayerManager.GetInstance().player.player_Point.ToString();
	}
public void setting(){
		if(Gun_menu.activeSelf){
			if(!preview_Camvas.activeSelf){//화면이 꺼져있는 경우
				preview_Camvas.SetActive(!preview_Camvas.activeSelf);	
				select_name.GetComponent<Text>().text = gun_name;
				select_explain.GetComponent<Text>().text = gun_explain;
				select_price.GetComponent<Text>().text = gun_price.ToString();
				select_image.GetComponent<Image>().sprite = gun_image;
				select_damage.GetComponent<Text>().text = gun_damage.ToString();
				kor.GetComponent<Text>().text = "공격력 : ";
			}else if(select_name.GetComponent<Text>().text!=gun_name){//화면이 켜져 있는데 선택한 gun이 아닌 경우
				select_name.GetComponent<Text>().text = gun_name;
				select_explain.GetComponent<Text>().text = gun_explain;
				select_price.GetComponent<Text>().text = gun_price.ToString();
				select_image.GetComponent<Image>().sprite = gun_image;
				select_damage.GetComponent<Text>().text = gun_damage.ToString();
				kor.GetComponent<Text>().text = "공격력 : ";
			}else{//화면이 켜져 있는데 선택한 것일 경우
				preview_Camvas.SetActive(false);
			}
		}else{
			if(!preview_Camvas.activeSelf){//화면이 꺼져있는 경우

				preview_Camvas.SetActive(!preview_Camvas.activeSelf);
				select_name.GetComponent<Text>().text = potion_name;
				select_explain.GetComponent<Text>().text = potion_explain;
				select_price.GetComponent<Text>().text = potion_price.ToString();
				select_image.GetComponent<Image>().sprite = potion_image;
				select_heal.GetComponent<Text>().text = potion_heal.ToString();
				kor.GetComponent<Text>().text = "회복력 : ";
			}else if(select_name.GetComponent<Text>().text!=potion_name){//화면이 켜져 있는데 선택한 gun이 아닌 경우
				select_name.GetComponent<Text>().text = potion_name;
				select_explain.GetComponent<Text>().text = potion_explain;
				select_price.GetComponent<Text>().text = potion_price.ToString();
				select_image.GetComponent<Image>().sprite = potion_image;
				select_heal.GetComponent<Text>().text = potion_heal.ToString();
				kor.GetComponent<Text>().text = "회복력 : ";
			}else{//화면이 켜져 있는데 선택한 것일 경우
				preview_Camvas.SetActive(false);
			}
		}
		SoundManager.I.PlaySFX("click");
	}

	public void active_menu(GameObject button){
		if(button.GetComponentInChildren<Text>().text.Equals("무기")){
			Gun_menu.SetActive(true);
			Potoin_menu.SetActive(false);
			preview_Camvas.SetActive(false);
		}else{
			Gun_menu.SetActive(false);
			Potoin_menu.SetActive(true);
			preview_Camvas.SetActive(false);
		}
		SoundManager.I.PlaySFX("click");
	}

	public void allBuy_check(){
		if(gunBuy_check==1 && potionBuy_check==1){
			block.SetActive(true);
			gunBuy_check=0;
			potionBuy_check=0;
		}

	}

}
