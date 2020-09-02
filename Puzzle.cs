using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour {
    public GameObject player;
    public GameObject canvas;
    public GameObject block1;
    public GameObject block2;
    public GameObject block3;
    public GameObject block4;
    public GameObject keyRing;

    private RectTransform rectTransform;
    private static int blockColCnt = 10;
    private static float blockWid;
    private static float block1x;
    private static float block1y;
    private static float firstRowy;

    private int maxHeight = 5;
    private int numOfPnds = 2;
    private static int[] currHeights;
    private static GameObject[,] blocks;

    void Start() {
        blocks = new GameObject[blockColCnt,maxHeight];
        rectTransform = canvas.GetComponent<RectTransform>();
        Build();
    }

    private void Build() {
        Vector3[] v = new Vector3[4];
        rectTransform.GetWorldCorners(v);

        block1x = v[0].x;
        block1y = v[0].y;

        BuildPuzzle(block1x,block1y);
    }

    private void BuildPuzzle(float x, float y) {
        float xPos = x+2;
        firstRowy = y+keyRing.GetComponent<Renderer>().bounds.size.y/2+1;
        float yPos = firstRowy;

        blockWid = block1.GetComponent<Renderer>().bounds.size.x;

        for(int i = 0; i < blockColCnt; i++) {
            Instantiate(block1, new Vector3(xPos,yPos,0), Quaternion.identity).tag = "firstrow";
            xPos += blockWid;
        }

        float ringX = keyRing.GetComponent<Renderer>().bounds.size.x/2 + xPos;
        
        Instantiate(block1, new Vector3(xPos,yPos,0), Quaternion.identity);
        Instantiate(keyRing, new Vector3(ringX,yPos,0), Quaternion.identity);

        float playerX = block1x+2;
        float playerY = block1y + keyRing.GetComponent<Renderer>().bounds.size.y/2+1 + blockWid*(maxHeight+1) + blockWid/4;
        Instantiate(player, new Vector3(playerX,playerY,0), Quaternion.identity);

        PuzzleGenerator pg = new PuzzleGenerator(maxHeight,blockColCnt,numOfPnds);
        currHeights = pg.GetPuzzleStart();
        AddPuzzleBlocks(currHeights);
    }

    private void AddPuzzleBlocks(int[] cols) {
        for(int i = 0; i < cols.Length; i++) {
            float x = block1x+2+(i*blockWid);
            float y = block1y+keyRing.GetComponent<Renderer>().bounds.size.y/2+1;

            for(int j = 0; j < cols[i]; j++) {
                y+=blockWid;

                if(j == cols[i]-1) { //top block
                    if(IsTrapezoid(i,cols)) {
                        blocks[i,j] = Instantiate(block2, new Vector3(x,y,0), Quaternion.identity);
                    } else if(IsLeftTriangle(i,cols)) {
                        blocks[i,j] = Instantiate(block3, new Vector3(x,y,0), Quaternion.identity);
                    } else if(IsRightTriangle(i,cols)) {
                        blocks[i,j] = Instantiate(block4, new Vector3(x,y,0), Quaternion.identity);
                    } else {
                        blocks[i,j] = Instantiate(block1, new Vector3(x,y,0), Quaternion.identity);
                    }
                } else { //square
                    blocks[i,j] = Instantiate(block1, new Vector3(x,y,0), Quaternion.identity);
                }
            }
        }
    }
 
    public static bool IsTrapezoid(int i, int[] cols) {
        if((i > 0 && i < cols.Length-1 && cols[i] > cols[i-1] && cols[i] > cols[i+1]) || (i == 0 && cols[i] > cols[i+1]) || (i == cols.Length-1 && cols[i] > cols[i-1]))
            return true;
        
        return false;
    }

    public static bool IsLeftTriangle(int i, int[] cols) {
        if((i < cols.Length-1 && cols[i] < cols[i+1]) || (i < cols.Length-2 && cols[i] == cols[i+1] && cols[i+1] > cols[i+2]))
            return true;
        
        return false;
    }

    public static bool IsRightTriangle(int i, int[] cols) {
        if((i > 0 && cols[i] < cols[i-1]) || (i > 2 && cols[i] == cols[i-1] && cols[i-1] > cols[i-2]))
            return true;
        
        return false;
    }

    public static float GetBlockWid() {
        return blockWid;
    }

    public static int GetBlockCnt() {
        return blockColCnt;
    }

    public static int[] GetCurHeights() {
        return currHeights;
    }

    public static GameObject[,] GetBlocks() {
        return blocks;
    }

    public static int[] GetAlteredHeights(float xPos, bool left) {
        int[] heights = (int[]) currHeights.Clone();
        int i = (int)Mathf.Floor((xPos-block1x)/blockWid)-2;

        heights[i]++;

        if(i > 0 && i < blockColCnt-1) { //middle
            if(left) {
                heights[i+1]--;
            } else {
                heights[i-1]--;
            }
        } else if(i == 0) { //leftmost
           if(left) {
                heights[i+1]--;
            } else {
                heights[blockColCnt-1]--;
            }
        } else if(i == blockColCnt-1) { //rightmost
            if(left) {
                heights[0]--;
            } else {
                heights[i-1]--;
            }
        }

        return heights;
    }

    public static int GetColFromPos(float xPos) {
        return (int)Mathf.Floor((xPos-block1x)/blockWid)-2;
    }

    public static float[] GetSideHeights(float xPos) {
        float[] heights = new float[3];
        int col = GetColFromPos(xPos);

        if(col > 0 && col < blockColCnt-1) { //middle
            heights[0] = firstRowy+((currHeights[col-1]+1)*blockWid);
            heights[1] = firstRowy+((currHeights[col+1]+1)*blockWid);
        } else if(col == 0) { //leftmost
            heights[0] = firstRowy+((currHeights[blockColCnt-1]+1)*blockWid);
            heights[1] = firstRowy+((currHeights[col+1]+1)*blockWid);
        } else if(col == blockColCnt-1) { //rightmost
            heights[0] = firstRowy+((currHeights[col-1]+1)*blockWid);
            heights[1] = firstRowy+((currHeights[0]+1)*blockWid);
        }
        
        heights[2] = (float)col;

        return heights;
    }

    public static void SetHeights(int idx, GameObject g1, GameObject g2) {
        blocks[idx,currHeights[idx]-1] = null;
        currHeights[idx]--;

        if(idx > 0 && idx < blockColCnt-1) { //middle
            blocks[idx-1, currHeights[idx-1]] = g1;
            blocks[idx+1, currHeights[idx+1]] = g2;
            currHeights[idx-1]++;
            currHeights[idx+1]++;
        } else if(idx == 0) { //leftmost
            blocks[blockColCnt-1,currHeights[blockColCnt-1]] = g1;
            blocks[idx+1,currHeights[idx+1]] = g2;
            currHeights[blockColCnt-1]++;
            currHeights[idx+1]++;
        } else if(idx == blockColCnt-1) { //rightmost
            blocks[idx-1,currHeights[idx-1]] = g1;
            blocks[0,currHeights[0]] = g2;
            currHeights[idx-1]++;
            currHeights[0]++;
        } 
    }
}
