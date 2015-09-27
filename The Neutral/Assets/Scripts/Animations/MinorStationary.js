#pragma strict


var attackTurnTime = 0.7;
var rotateSpeed = 120.0;
var attackDistance = 17.0;
var extraRunTime = 2.0;

var damage = 1;
var attackSpeed = 1.0;
var attackRotateSpeed = 20.0;
var idleTime = 1.6;
var punchPosition = new Vector3 (0.4, 0, 0.7);
var punchRadius = 1.1;
private var attackAngle = 10.0;
private var isAttacking = false;
private var lastPunchTime = 0.0;
var target : Transform;
private var characterController : CharacterController;
characterController = GetComponent(CharacterController);


function Start () {

	//GetComponent.<Animation>().wrapMode = WrapMode.Loop;
}

/*
function Idle() {
	GetComponent.<Animation>().Play('Respawn');
	yield WaitForSeconds(idleTime);
}
*/

/*
while (true) {
	yield Idle();
}
*/


function Update () {

	if (Input.GetKey("a")) { GetComponent.<Animation>().Play('Respawn'); }
	else if (Input.GetKey("s")) { GetComponent.<Animation>().Play('Death'); }
	else if (Input.GetKey("d")) { GetComponent.<Animation>().Play('Merge'); }
	else if (Input.GetKey("f")) { GetComponent.<Animation>().Play('Run'); }
	else if (Input.GetKey("g")) { GetComponent.<Animation>().Play('Idle'); } 

}