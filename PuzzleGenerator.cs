using System;
using System.Collections;
using System.Collections.Generic;

public class PuzzleGenerator {
    private int maxHeight;
    private int numOfCols;
    private int numOfPnds;
    private int[] solution;
    private int[] start;

    public PuzzleGenerator(int maxh, int cols, int pnds) {
        maxHeight = maxh;
        numOfCols = cols;
        numOfPnds = pnds;

        solution = CreateSolution();
        start = CreateStart(solution);
    }

    private int[] CreateSolution() {
        int[] solution = new int[numOfCols];
        Random rand = new Random();

        for(int i = 0; i < numOfCols; i++) {
            solution[i] = rand.Next(0,maxHeight);
        }

        return solution;
    }

    private int[] CreateStart(int[] s) {
        int[] start = (int[]) solution.Clone();
        Random rand = new Random();

        for(int i = 0; i < numOfPnds; i++) {
            int col = rand.Next(0,numOfCols);

            start[col]--;

            if(col == 0) {
                start[numOfCols-1]++;
                start[col+1]++;
            } else if(col == numOfCols-1) {
                start[col-1]++;
                start[0]++;
            } else {
                start[col-1]++;
                start[col+1]++;
            }
        }

        return start;
    }

    public int[] GetPuzzleSolution() {
        return solution;
    }

    public int[] GetPuzzleStart() {
        return start;
    }
}
