using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Sprite player;
    public Sprite fallingPlayer;

    private SpriteRenderer spriteRenderer; 
    private Rigidbody2D rigidBody;

    private float blockWid;
    private int blockCnt;
    private int pos = 0;
    private float originalY;
    private bool falling = false;

    void Start() { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.simulated = false;
        rigidBody.velocity = new Vector2(0, 0);

        Vector3 position = transform.position;
        originalY = position.y;

        blockWid = Puzzle.GetBlockWid();
        blockCnt = Puzzle.GetBlockCnt();
    }

    void Update() {
        if(falling == false) {
            rigidBody.velocity = new Vector2(0, -2);
            
            if((Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A))) && pos > 0) {
                rigidBody.simulated = false;

                Vector3 newPosition = transform.position;
                newPosition.x -= blockWid;
                transform.position = newPosition; 
                pos--;
            } else if((Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D))) && pos < blockCnt-1) {
                rigidBody.simulated = false;

                Vector3 newPosition = transform.position;
                newPosition.x += blockWid;
                transform.position = newPosition; 
                pos++;
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Return))) {
                falling = true;
                spriteRenderer.sprite = fallingPlayer;
                rigidBody.simulated = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "block" || col.tag == "firstrow") {
            rigidBody.simulated = false;
            spriteRenderer.sprite = player;

            Vector3 newPosition = transform.position;
            newPosition.y = originalY;
            transform.position = newPosition;
            falling = false; 
        }
    }
}
