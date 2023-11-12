using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Utils
{
    /** <returns>An angle in which a 2D object at origin will be facing target</returns> */
    public static float RotateTowardsDegrees2D(Vector2 target, Vector2 origin)
    {
        return Mathf.Atan2(target.y - origin.y, target.x - origin.x) * Mathf.Rad2Deg;
    }

    /** <returns>An angle in which a 2D object at (0, 0) will be facing target</returns> */
    public static float RotateTowardsDegrees2D(Vector2 target)
    {
        return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
    }

    /** <returns>A quaternion in which a 2D object at origin will be facing target</returns> */
    public static Quaternion RotateTowardsQuaternion2D(Vector2 target, Vector2 origin)
    {
        return Quaternion.Euler(0, 0, RotateTowardsDegrees2D(target, origin));
    }

    /** <returns>A quaternion in which a 2D object at (0, 0) will be facing target</returns> */
    public static Quaternion RotateTowardsQuaternion2D(Vector2 target)
    {
        return Quaternion.Euler(0, 0, RotateTowardsDegrees2D(target));
    }

    /** <returns>A normalized vector in the direction of an angle</returns> */
    public static Vector2 AngleToVectorDegrees(float degrees)
    {
        return AngleToVectorRadians(degrees * Mathf.Deg2Rad);
    } 
    
    /** <returns>A normalized vector in the direction of an angle</returns> */
    public static Vector2 AngleToVectorRadians(float radians)
    {
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    } 

    /** <returns>An angle to be added to the projectiles angle in order to collide with the target if target's velocity is constant</returns> */
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

    /** <returns>A line renderer component in the shape of a circle</returns> */
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

    /** <returns>A color with the givenalpha */
    public static Color ColorWithAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
