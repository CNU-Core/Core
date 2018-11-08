using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class M416 : MonoBehaviour {

	public string explain = "이것은 M416이다.";
	public int price = 200; 
	public string name = "M416";
	public int damage = 200;
	public Sprite image;

public void setData(){
		ShopManager.Instance.gun_price = this.price;
		ShopManager.Instance.gun_damage = this.damage;
		ShopManager.Instance.gun_explain = this.explain;
		ShopManager.Instance.gun_name = this.name;
		ShopManager.Instance.gun_image = this.image;
		ShopManager.Instance.setting();
	}
	public int Price{
		get{
			return price;
		}
	}
	public int Damage{
		get{
			return damage;
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
