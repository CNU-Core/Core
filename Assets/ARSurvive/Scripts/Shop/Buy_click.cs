using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Buy_click : MonoBehaviour {
	public GameObject GunMenu;
	public GameObject PotionMenu;
	public Button GunButton;
	public Button PotionButton;
	public GameObject Select_menu;

	public Text current_poiont;
	public void buyClick(){
		
		if(GunMenu.activeSelf){
			ShopManager.instance.gunBuy_check=1;
			PotionMenu.SetActive(true);
			GunMenu.SetActive(false);
			Select_menu.SetActive(false);
			GunButton.GetComponent<Button>().enabled = false;
			GamesManager.Instance.point = GamesManager.Instance.point - ShopManager.Instance.gun_price;
			

		}else{
			ShopManager.instance.potionBuy_check=1;
			PotionMenu.SetActive(false);
			GunMenu.SetActive(true);
			Select_menu.SetActive(false);
			PotionButton.GetComponent<Button>().enabled = false;
			GamesManager.Instance.point = GamesManager.Instance.point - ShopManager.instance.potion_price;
		}
		current_poiont.GetComponent<Text>().text =  GamesManager.Instance.point.ToString();
		ShopManager.instance.allBuy_check();
	}
}
