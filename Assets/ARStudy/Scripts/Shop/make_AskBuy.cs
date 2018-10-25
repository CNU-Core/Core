using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class make_AskBuy : MonoBehaviour {
	//public Image selectedGun_Image;
	public Image show_Image;
	public Text show_price;
	public Text show_name;
	public void make_AskSelect_window(){
		show_name.GetComponent<Text>().text = ShopManager.instance.select_name.GetComponent<Text>().text;
		show_price.GetComponent<Text>().text = ShopManager.instance.select_price.GetComponent<Text>().text;
		show_Image.GetComponent<Image>().sprite = ShopManager.instance.select_image.GetComponent<Image>().sprite;
	}


}
