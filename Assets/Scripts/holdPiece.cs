using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdPiece : MonoBehaviour {
    public GameObject GameLogic;
    public GameObject raycastHolder;
    public GameObject player;
    public GameObject pieceBeingHeld;
	public GameObject gravityAttractor;
    public bool holdingPiece = false;
    public float hoverHeight = 0.3f;
    GameLogic gLogic;
    Rigidbody rBody;
    BoxCollider bCollider;
    PlayerPiece pPiece;
    RaycastHit hit;
	public float gravityFactor = 10.0f;
	private Vector3 forceDirection;

    // Use this for initialization
    void Start () {
        gLogic = GameLogic.GetComponent<GameLogic>();

    }
	public void grabPiece(GameObject selectedPiece) {
        if (selectedPiece.GetComponent<PlayerPiece>().hasBeenPlayed == false) {
            pieceBeingHeld = selectedPiece;
            rBody = pieceBeingHeld.GetComponent<Rigidbody>();
            bCollider = pieceBeingHeld.GetComponent<BoxCollider>();
            pPiece = pieceBeingHeld.GetComponent<PlayerPiece>();
            holdingPiece = true;
        }
       
    }
	// Update is called once per frame
	void FixedUpdate () {
        if (gLogic.playerTurn == true) {
            if (holdingPiece == true) {
                Vector3 forwardDir = raycastHolder.transform.TransformDirection(Vector3.forward) * 100;
                Debug.DrawRay(raycastHolder.transform.position, forwardDir, Color.green);


                if (Physics.Raycast(raycastHolder.transform.position, (forwardDir), out hit)) {
					gravityAttractor.transform.position = new Vector3(hit.point.x, hit.point.y + hoverHeight, hit.point.z);


                    rBody.useGravity = false;
					bCollider.enabled = false;

                    rBody.AddForce(gravityAttractor.transform.position - pieceBeingHeld.transform.position);


                    if (hit.collider.gameObject.tag == "Grid Plate") {
                       GameObject selectedGridPlates = hit.collider.gameObject;
                        if (Input.GetMouseButtonDown(0) && gLogic.isChekGridPlateOpen(selectedGridPlates)) {
                            pPiece.transform.position = new Vector3(selectedGridPlates.transform.position.x, selectedGridPlates.transform.position.y, selectedGridPlates.transform.position.z);
                            holdingPiece = false;
                            hit.collider.gameObject.SetActive(false);
                            pPiece.hasBeenPlayed = true;
                            rBody.useGravity = true;
							bCollider.enabled = true;
                            gLogic.playerMove(selectedGridPlates);
                        }

                    }

                }
            }
        }
    }

}







