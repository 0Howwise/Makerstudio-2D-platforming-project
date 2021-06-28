using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string loadLevel;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        loadLevel = "Level 2";
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Player")) {
            audioSource.Play();
            SceneManager.LoadScene(loadLevel);
        }
    }
}
