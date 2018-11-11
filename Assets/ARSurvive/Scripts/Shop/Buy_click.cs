using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Buy_click : MonoBehaviour {
	public GameObject GunMenu;
	public GameObject PotionMenu;
	public GameObject cantBuy;
	public Button GunButton;
	public Button PotionButton;
	public GameObject Select_menu;
	private PlayerManager playerManager;
	public Text current_poiont;

	void Start(){
		playerManager = PlayerManager.GetInstance();
	}
	public void buyClick(){
		
		if(GunMenu.activeSelf){
			//Debug.Log(playerManager.player.player_Point);
			if(playerManager.player.player_Point
			- ShopManager.Instance.gun_price >= 0){
				ShopManager.instance.gunBuy_check=1;
				PotionMenu.SetActive(true);
				GunMenu.SetActive(false);
				Select_menu.SetActive(false);
				GunButton.GetComponent<Button>().enabled = false;
				playerManager.player.player_Point = playerManager.player.player_Point - ShopManager.Instance.gun_price;
				}
			else{
				cantBuy.SetActive(true);
			}
		}else{
			if(playerManager.player.player_Point - ShopManager.Instance.gun_price >= 0){
				ShopManager.instance.potionBuy_check=1;
				PotionMenu.SetActive(false);
				GunMenu.SetActive(true);
				Select_menu.SetActive(false);
				PotionButton.GetComponent<Button>().enabled = false;
				playerManager.player.player_Point = playerManager.player.player_Point - ShopManager.instance.potion_price;
			}else{
				cantBuy.SetActive(true);
			}
		}
		current_poiont.GetComponent<Text>().text =  playerManager.player.player_Point.ToString();
		ShopManager.instance.allBuy_check();
	}
}
