using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PotionRed : MonoBehaviour {
	public string explain = "이것은 빨간 포션이다.";
	public int price = 100; 
	public string name = "Red";
	public int heal = 100;
	public Sprite image;

		public void setData(){
		ShopManager.Instance.potion_price = this.price;
		ShopManager.Instance.potion_heal = this.heal;
		ShopManager.Instance.potion_explain = this.explain;
		ShopManager.Instance.potion_name = this.name;
		ShopManager.Instance.potion_image = this.image;
		ShopManager.Instance.setting();
	}

	public int Price{
		get{
			return price;
		}
	}
	public int Heal{
		get{
			return heal;
		}
	}
	public string Explain{
		get{
			return explain;
		}
	}

	public string Name{
		get{
			return name;
		}
	}
	
}
