using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AKM : MonoBehaviour {
	public string explain = "이것은 AKM이다.";
	public int price = 100; 
	public string name = "AKM";
	public int damage = 100;
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
