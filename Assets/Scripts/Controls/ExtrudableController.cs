using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
	public class ExtrudableController : IInteractableObject
	{

        private Quaternion startRotation;
        private Quaternion initialRotation;
        // scale and translate
        private Vector3 initialPosition;
        private Vector3 initialHandPosition;
        private Quaternion initialHandRotation;
        private Vector3 initialControllerCenter;
        public float initialScale;
        public float initialDistanceBetweenControllers;


        public Transform handTransform;

        private InteractionMode mode = InteractionMode.SINGLE;

        public ControlsManager controlsManager;

        public List<GrabControl> activeControllers = new List<GrabControl>();
        private int updateInFrame;

        private Vector3 grabOffsetLocalPosition;
        private Quaternion grabOffsetLocalRotation;
        private GameObject LeftHand;
        private GameObject RightHand;
        private Vector3 smoothedLeftHandPosition;
        private Vector3 smoothedRightHandPosition;
        private float smoothingSpeed = 20.0f; // Tune this value higher for tighter tracking, lower for more smoothing


        private bool isGrabbing = false;

        private Vector3 grabOffsetPosition;
        private Quaternion grabOffsetRotation;

        private Rigidbody rb;

        // Use this for initialization
        void Awake()
		{
            //controlsManager = FindObjectOfType<ControlsManager>();
            LeftHand = GameObject.Find("LeftHand");
            RightHand = GameObject.Find("RightHand");
            rb = GetComponent<Rigidbody>();
        }


		// Update is called once per frame
		void Update()
        {
            Vector3 left_ctrl_pt = LeftHand.transform.position;
            Vector3 right_ctrl_pt = RightHand.transform.position;

            float dist_left_ctrl = (10.0f * (left_ctrl_pt - transform.position)).sqrMagnitude;
            float dist_right_ctrl = (10.0f * (right_ctrl_pt - transform.position)).sqrMagnitude;
            float sz = 0.1f + 0.9f / (Mathf.Min(dist_left_ctrl, dist_right_ctrl) + 1.0f);
            //transform.localScale = new Vector3(sz, sz, sz);

            if (updateInFrame == Time.frameCount) return;
            //Currently active grabbing controllers
            //if only one, we just want to move
            if (activeControllers.Count == 1)
            {
                var controlsRoot = controlsManager.Extrudable.transform.root;
                var deltaMovement = activeControllers[0].transform.position - initialHandPosition;
                //controlsManager.Extrudable.transform.root.transform.position = initialPosition + deltaMovement;

                Quaternion deltaRot = activeControllers[0].transform.rotation * Quaternion.Inverse(initialHandRotation);
                controlsRoot.position = RotatePointAroundPivot(initialPosition + deltaMovement, activeControllers[0].transform.position, deltaRot);
                controlsRoot.rotation =  deltaRot * initialRotation;

                
                //controls.UpdateControls();


                /*controlsManager.Extrudable.rebuild = true;
                controlsManager.UpdateControls();*/
                
            }
            //If two grabs, then scale the mesh
            else if (activeControllers.Count == 2)
            {
                float distanceBetweenControllers = Vector3.Distance(activeControllers[1].transform.position,
                            activeControllers[0].transform.position);

                controlsManager.SetGlobalScale(initialScale * (distanceBetweenControllers / initialDistanceBetweenControllers));

                var scaleMovement = (initialPosition - initialControllerCenter) * ((distanceBetweenControllers / initialDistanceBetweenControllers)-1);

                var deltaMovement = (activeControllers[1].transform.position +
                                     activeControllers[0].transform.position) * 0.5f -
                                    initialControllerCenter;
                controlsManager.Extrudable.transform.root.position =initialPosition + scaleMovement+ deltaMovement;

            }

            controlsManager.Extrudable.rebuild = true;
            controlsManager.UpdateControls();
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.identity;
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            return rotation * (point - pivot) + pivot;
        }

        public override void UpdateHandleSize()
        {
            return;
		}

        public override void Interact()
        {
            updateInFrame = Time.frameCount;
            handTransform = _controllerCollider.transform.parent;
            activeControllers.Add(_controller.GetComponent<GrabControl>());


            if (activeControllers.Count == 1)
            {
                grabOffsetLocalPosition = handTransform.InverseTransformPoint(transform.position);
                grabOffsetLocalRotation = Quaternion.Inverse(handTransform.rotation) * transform.rotation;
                InitRotationInteraction();
            }
            else if (activeControllers.Count == 2)
            {
                InitScaleInteraction();
                //Debug.Break();   
            }
        }

        private void InitScaleInteraction()
        {
            initialScale = transform.parent.localScale.x;
            initialDistanceBetweenControllers = Vector3.Distance(activeControllers[1].transform.position,
                            activeControllers[0].transform.position);
            initialControllerCenter = (activeControllers[1].transform.position +
                                       activeControllers[0].transform.position) * .5f;
            initialPosition = transform.position;
        }

        private void InitRotationInteraction()
        {
            initialPosition = transform.position;
            initialHandPosition = activeControllers[0].transform.position;
            initialRotation = transform.rotation;
            initialHandRotation = activeControllers[0].transform.rotation;
        }

        public override void ChangeInteraction(InteractionMode mode)
        {

        }

        public override void StopInteraction()
        {

            /*
                        activeControllers.Remove(_controller.GetComponent<GrabControl>());

                        if (activeControllers.Count == 1)
                        {
                            InitRotationInteraction();
                        }*/

            // Ensure this runs only once per frame
            if (updateInFrame == Time.frameCount)
                return;

            updateInFrame = Time.frameCount;

            var grabControl = _controller as GrabControl;
            if (grabControl != null && activeControllers.Contains(grabControl))
            {
                // Final update of mesh transform based on last hand pose
                if (activeControllers.Count == 1)
                {
                    var controller = activeControllers[0];
                    var deltaMovement = controller.transform.position - initialHandPosition;
                    controlsManager.Extrudable.transform.root.position = initialPosition + deltaMovement;

                    Quaternion deltaRot = controller.transform.rotation * Quaternion.Inverse(initialHandRotation);
                    controlsManager.Extrudable.transform.root.position =
                        RotatePointAroundPivot(controlsManager.Extrudable.transform.root.position, controller.transform.position, deltaRot);
                    controlsManager.Extrudable.transform.root.rotation = deltaRot * initialRotation;

                    //controlsManager.Extrudable.rebuild = true;
                    //controlsManager.UpdateControls();
                }

                transform.localPosition = new Vector3(0, 0, 0);
                transform.localRotation = Quaternion.identity;
                activeControllers.Remove(grabControl);
            }
        }

        public override void StartHighlight()
        {

        }

        public override void EndHighlight()
        {

        }

        public void InteractableUpdate()
        {
            Vector3 left_ctrl_pt = LeftHand.transform.position;
            Vector3 right_ctrl_pt = RightHand.transform.position;

            float dist_left_ctrl = (10.0f * (left_ctrl_pt - transform.position)).sqrMagnitude;
            float dist_right_ctrl = (10.0f * (right_ctrl_pt - transform.position)).sqrMagnitude;
            float sz = 0.1f + 0.9f / (Mathf.Min(dist_left_ctrl, dist_right_ctrl) + 1.0f);
            //transform.localScale = new Vector3(sz, sz, sz);

            if (updateInFrame == Time.frameCount) return;
            //Currently active grabbing controllers
            //if only one, we just want to move
            if (activeControllers.Count == 1)
            {
                var deltaMovement = activeControllers[0].transform.position - initialHandPosition;
                controlsManager.Extrudable.transform.root.transform.position = initialPosition + deltaMovement;

                Quaternion deltaRot = activeControllers[0].transform.rotation * Quaternion.Inverse(initialHandRotation);
                controlsManager.Extrudable.transform.root.position = RotatePointAroundPivot(controlsManager.Extrudable.transform.root.position, activeControllers[0].transform.position, deltaRot);
                controlsManager.Extrudable.transform.root.rotation = deltaRot * initialRotation;


                //controls.UpdateControls();


                controlsManager.Extrudable.rebuild = true;
                controlsManager.UpdateControls();

            }
            //If two grabs, then scale the mesh
            else if (activeControllers.Count == 2)
            {
                float distanceBetweenControllers = Vector3.Distance(activeControllers[1].transform.position,
                            activeControllers[0].transform.position);

                controlsManager.SetGlobalScale(initialScale * (distanceBetweenControllers / initialDistanceBetweenControllers));

                var scaleMovement = (initialPosition - initialControllerCenter) * ((distanceBetweenControllers / initialDistanceBetweenControllers) - 1);

                var deltaMovement = (activeControllers[1].transform.position +
                                     activeControllers[0].transform.position) * 0.5f -
                                    initialControllerCenter;
                controlsManager.Extrudable.transform.root.position = initialPosition + scaleMovement + deltaMovement;

            }
        }

    }
}
