using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Buy_click : MonoBehaviour {
	public GameObject Buy_Canvas;
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
			int price = int.Parse(ShopManager.instance.select_price.GetComponent<Text>().text.ToString());
			Debug.Log(price);
			if(price <= PlayerManager.GetInstance().player.player_Point){
				ShopManager.instance.gunBuy_check=1;
				PlayerManager.GetInstance().player.BulletPower = int.Parse(ShopManager.instance.select_damage.GetComponent<Text>().text.ToString());
				PlayerManager.GetInstance().SubPlayerPoint(price);
				ShopManager.instance.UpdatePoint();
				ObjManager.Call().PlayerInfoUpdate(); //총알의 각각의 파워를 정의
				PotionMenu.SetActive(true);
				GunMenu.SetActive(false);
				Select_menu.SetActive(false);
				GunButton.GetComponent<Button>().enabled = false;
				playerManager.player.player_Point = playerManager.player.player_Point - ShopManager.Instance.gun_price;
				SoundManager.I.PlaySFX("charge");
				current_poiont.GetComponent<Text>().text =  playerManager.player.player_Point.ToString();
				ShopManager.instance.allBuy_check();
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
				SoundManager.I.PlaySFX("charge");
				current_poiont.GetComponent<Text>().text =  playerManager.player.player_Point.ToString();
				ShopManager.instance.allBuy_check();
			}else{
				cantBuy.SetActive(true);
			}
		}
		
		
	}
}
