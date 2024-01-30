using UnityEngine;
using System;

public class Follow_Coordinates : MonoBehaviour
{

    [SerializeField]
    private Transform plate2;
    [SerializeField]
    private Transform joint1;
    [SerializeField]
    private Transform joint2;  
    [SerializeField]
    private Transform Target_Ball;
    [SerializeField]
    private float arm_length = 2.9f;  
    [SerializeField]
    private float speed = 5f;
    private double XZlength;
    private double hlength;
    private double angle_X;
    private double angle_XZ;
    private double angle_theta;
    private double angle_theta_full;
    private double angle_phi;
    private double angle_phi_full;
    [SerializeField]
    private bool ManunalInsertion = false;
    public Vector3 target;
   



    // Update is called once per frame
    void FixedUpdate()
    {
        findTarget();
        lengths();
        angles();
        rotateToTarget();




    }
    void findTarget()
    {
        if (ManunalInsertion)
        {
            Target_Ball.position = target;

        }
        else
        {
            target = Target_Ball.position;
        }
    }
    void lengths()
    {

        //calculate XZ projection of Target Vector
        XZlength = Math.Sqrt(Math.Pow(target.x, 2) + Math.Pow(target.z, 2));
        if (XZlength> 2 * arm_length)
        {
            XZlength= 2 * arm_length;
        }

        //calculate Length of the Target Vector
        hlength = Math.Sqrt(Math.Pow(XZlength, 2) + Math.Pow(target.y, 2));
        if(hlength>2 * arm_length )
        {
            hlength = 2 * arm_length;
        }
    }
    void angles()
    {
        //angle_X is the angle between the projection on XZ plane and the x-axis
        //angle_XZ is the angle between the Target Vector and the projection on XZ plane
        //angle_phi is the angle of joint 2
        //angle_theta_full is theh angle of joint 1
        
        angle_X = -Math.Atan2(target.z, target.x) * Mathf.Rad2Deg;
        angle_XZ = -Math.Atan2(XZlength , target.y) * Mathf.Rad2Deg;
        angle_phi = Math.Acos((Math.Pow(arm_length, 2) + Math.Pow(arm_length, 2)
                    - Math.Pow(hlength, 2)) / (2 * arm_length * arm_length)) * -Mathf.Rad2Deg;
        angle_theta = Math.Acos((Math.Pow(arm_length, 2) + Math.Pow(hlength, 2)
                    - Math.Pow(arm_length, 2)) / (2 * arm_length * hlength)) * Mathf.Rad2Deg;
        angle_phi_full = 180f - angle_phi + angle_theta_full;
        angle_theta_full = angle_theta + angle_XZ;
    }
    void rotateToTarget()
    {
        Quaternion angle_X_Q = Quaternion.Euler(0, (float)angle_X, 0);
        Quaternion angle_XZ_Q = Quaternion.Euler(0, (float)angle_X, (float)angle_XZ);
        Quaternion angle_phi_full_Q = Quaternion.Euler(0, (float)angle_X, (float)angle_phi_full);
        Quaternion angle_theta_full_Q = Quaternion.Euler(0, (float)angle_X, (float)angle_theta_full);

        
        plate2.rotation = Quaternion.Lerp(plate2.rotation, angle_X_Q, Time.deltaTime * speed);
        joint1.rotation = Quaternion.Lerp(joint1.rotation, angle_theta_full_Q, Time.deltaTime * speed);
        joint2.rotation = Quaternion.Lerp(joint2.rotation, angle_phi_full_Q, Time.deltaTime * speed);


    }

   
}