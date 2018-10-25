using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class activeMenu : MonoBehaviour {
	public void setMenu(){
		ShopManager.Instance.active_menu(gameObject);
	}

}
