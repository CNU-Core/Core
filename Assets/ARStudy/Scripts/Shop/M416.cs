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
