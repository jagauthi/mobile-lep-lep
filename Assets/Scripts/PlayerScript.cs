using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    ArrayList abilities; 
    string test;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilities = new ArrayList();
        abilities.Add("Smash :3");
        abilities.Add("gwab");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void printAbilities() {
        foreach (string ability in abilities) {
            Debug.Log(ability);
        }
    }
}
