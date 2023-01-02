using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Utils
{
    /** Returns an angle in which a 2D object at origin will be facing target */
    public static float rotateTowardsDegrees2D(Vector2 target, Vector2 origin)
    {
        return Mathf.Atan2(target.y - origin.y, target.x - origin.x) * Mathf.Rad2Deg;
    }

    /** Returns an angle in which a 2D object at (0, 0) will be facing target */
    public static float rotateTowardsDegrees2D(Vector2 target)
    {
        return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
    }

    /** Returns a quaternion in which a 2D object at origin will be facing target */
    public static Quaternion rotateTowardsQuaternion2D(Vector2 target, Vector2 origin)
    {
        return Quaternion.Euler(0, 0, rotateTowardsDegrees2D(target, origin));
    }

    /** Returns a quaternion in which a 2D object at (0, 0) will be facing target */
    public static Quaternion rotateTowardsQuaternion2D(Vector2 target)
    {
        return Quaternion.Euler(0, 0, rotateTowardsDegrees2D(target));
    }

    /** Returns an angle to be added to the projectiles angle in order to collide with the target if velocity is constant */
    public static float ProjectileTrajectoryPrediction(Vector2 targetPos, Vector2 targetVel, float targetAngle, Vector2 projectilePos, float projectileVel, float projectileAngle)
    {
        // a b and c values of quadratic equation ax^2 + bx + c = 0
        float a = (targetVel.x * targetVel.x) +
            (targetVel.y * targetVel.y) - (projectileVel * projectileVel);

        float b = 2 * (targetVel.x *
                       (targetPos.x - projectilePos.x)
                       + targetVel.y *
                       (targetPos.y - projectilePos.y));

        float c = ((targetPos.x - projectilePos.x) *
                   (targetPos.x - projectilePos.x)) +
                  ((targetPos.y - projectilePos.y) *
                   (targetPos.y - projectilePos.y));

        // Number under sqrt of quadratic formula
        float discriminant = b * b - (4 * a * c);

        // Check if roots are imaginary
        if (discriminant < 0)
            return 0;

        // Times the projectile would collide with the bullet
        float t1 = (-1 * b + Mathf.Sqrt(discriminant)) / (2 * a);
        float t2 = (-1 * b - Mathf.Sqrt(discriminant)) / (2 * a);

        // Take the larger time value and clamp to positive
        float time = Mathf.Max(Mathf.Max(t1, t2), 0);
        
        // Position to aim projectile
        float aimX = (targetVel.x * time) + targetPos.x - projectilePos.x;
        float aimY = (targetVel.y * time) + targetPos.y - projectilePos.y;
        
        // Projectile angle to hit target
        return Mathf.Atan2(aimY, aimX) * Mathf.Rad2Deg;
    }

    public static void GenerateLineRendCircle(LineRenderer lineRend, float radius, int numPoints)
    {
        lineRend.positionCount = numPoints + 1;
        Vector3[] positions = new Vector3[numPoints + 1];

        for (int i = 0; i <= numPoints; i++)
        {
            float rad = Mathf.PI * 2f * ((float)i / numPoints);
            positions[i] = new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0);
        }
        lineRend.SetPositions(positions);
    }
}
