using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public Sprite b1;
    public Sprite b2;
    public Sprite b3;
    public Sprite b4;

    private float xPos;
    private float yPos;
    private float width;
    private int colCnt;

    void Start() {        
        Vector3 position = transform.position;
        xPos = position.x;
        yPos = position.y;
        width = Puzzle.GetBlockWid(); 
        colCnt = Puzzle.GetBlockCnt();  
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "player" && gameObject.tag != "firstrow") {  
            float[] h = Puzzle.GetSideHeights(xPos);

            float x1 = xPos-width;
            float x2 = xPos+width;

            if(h[2] == 0) { //leftmost
                x1 = xPos+(width*(colCnt-1));
            } else if(h[2] == colCnt-1) { //rightmost
                x2 = xPos-(width*(colCnt-1));  
            }

            GameObject g1 = AddSideBlock(true,x1,h[0]);
            GameObject g2 = AddSideBlock(false,x2,h[1]);

            Destroy(gameObject);
            
            Puzzle.SetHeights((int)h[2],g1,g2);
            UpdateBlocks(Puzzle.GetCurHeights(),Puzzle.GetBlocks(),g1,g2);
        }
    }

    private GameObject AddSideBlock(bool leftmost, float x, float y) {
        GameObject block = gameObject;
        SpriteRenderer spriteRenderer = block.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = b1;
        Instantiate(block, new Vector3(x,y,0), Quaternion.identity);
        return block;
    }

    private void UpdateBlocks(int[] cols, GameObject[,] blocks) {
        for(int i = 0; i < cols.Length; i++) {
            for(int j = 0; j < cols[i]; j++) {
                GameObject block = blocks[i,j];

                if(block != null) {
                    SpriteRenderer spriteRenderer = block.GetComponent<SpriteRenderer>();

                    if(j == cols[i]-1) {
                        if(Puzzle.IsTrapezoid(i,cols)) {
                            spriteRenderer.sprite = b2;
                        } else if(Puzzle.IsLeftTriangle(i,cols)) {
                            spriteRenderer.sprite = b3;
                        } else if(Puzzle.IsRightTriangle(i,cols)) {
                            spriteRenderer.sprite = b4;
                        } else {
                            spriteRenderer.sprite = b1;
                        }
                    } else {
                        spriteRenderer.sprite = b1;
                    }
                } else {
                    Debug.Log(i + " " + j);
                }
            }
        }
    }
}
