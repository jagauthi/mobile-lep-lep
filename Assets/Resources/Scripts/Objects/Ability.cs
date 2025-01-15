using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability {
	string name, type;
	int resourceCost, power;
	GameObject gameObject;
	Texture2D icon;

	public Ability(string name, string type, int resourceCost, int power, GameObject gameObject, Texture2D icon) {
		this.name = name;
		this.type = type;
		this.resourceCost = resourceCost;
		this.power = power;
		this.gameObject = gameObject;
		this.icon = icon;
	}

	public string getName() {
		return name;
	}

	public string getType() {
		return type;
	}

	public int getResourceCost() {
		return resourceCost;
	}

	public int getPower() {
		return power;
	}

	public GameObject getGameObject() {
		return gameObject;
	}

	public Texture2D getIcon() {
		return icon;
	}

	public Rigidbody getBody() {
		return gameObject.GetComponent<Rigidbody>();
	}
}
