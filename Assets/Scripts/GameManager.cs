using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private struct Move {
        public Mover mover;
        public Vector3 direction;
    }
    
    public static int score = 0;
    public static Mover selectedMover;
    
    public TextMeshProUGUI scoreText;
    public static GameObject selectBox;
    private Collider2D[] colls = new Collider2D[4];

    private Stack<Move> moveHistory = new Stack<Move>();
    public float replayStepTime = 0.3f;

    public bool isPlaying = true;

    private void Start() {
        selectedMover = FindObjectOfType<Mover>();
        selectBox = GameObject.FindWithTag("Player");
    }

    private void Update() {

        if (isPlaying) {

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (CheckMove(Vector3.up))
                    score += selectedMover.cost;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (CheckMove(Vector3.down))
                    score += selectedMover.cost;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (CheckMove(Vector3.right))
                    score += selectedMover.cost;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (CheckMove(Vector3.left))
                    score += selectedMover.cost;
            }


            if (Input.GetKeyDown(KeyCode.A)) {
                selectedMover.UpdateType(Mover.ObjType.a);
            }

            if (Input.GetKeyDown(KeyCode.B)) {
                selectedMover.UpdateType(Mover.ObjType.b);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                selectedMover.UpdateType(Mover.ObjType.c);
            }

            if (Input.GetKeyDown(KeyCode.D)) {
                selectedMover.UpdateType(Mover.ObjType.d);
            }

            if (Input.GetKeyDown(KeyCode.Z)) {
                UndoMove();
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                isPlaying = false;
                StartCoroutine(Replay());
            }
        }
        
        
        if (Input.GetKeyDown(KeyCode.R)) {
            StopAllCoroutines();
            score = 0;
            moveHistory.Clear();
            foreach (Mover mover in FindObjectsOfType<Mover>()) {
                mover.Reset();
            }
        }
        
        scoreText.text = "Score: " + score;
        selectBox.transform.position = selectedMover.transform.position;
    }

    private IEnumerator Replay() {
        score = 0;
        foreach (Mover m in FindObjectsOfType<Mover>()) {
            m.Reset();
        }

        yield return new WaitForSeconds(replayStepTime);
        
        foreach (Move m in moveHistory.Reverse()) {
            selectedMover = m.mover;
            m.mover.transform.Translate(m.direction);
            score += m.mover.cost;
            yield return new WaitForSeconds(replayStepTime);
        }

        isPlaying = true;
    }
    
    private bool CheckMove(Vector3 direction) {
        if (Physics2D.OverlapBoxNonAlloc(selectedMover.transform.position + direction, new Vector2(0.5f, 0.5f), 0, colls) !=
            0) {
            return false;
        }

        Move move;
        move.mover = selectedMover;
        move.direction = direction;
        
        moveHistory.Push(move);
        selectedMover.transform.Translate(direction);
        return true;
    }

    private void UndoMove() {
        if (moveHistory.Count <= 0) return;

        Move move = moveHistory.Pop();
        selectedMover = move.mover;
        selectedMover.transform.Translate(-move.direction);
        score -= move.mover.cost;
    }
    
}
