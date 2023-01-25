using Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestPlugin
{
    static class CornersRounder
    {
        private const int CornerSections = 5;
        public static List<Vector3> RoundCorners(List<Vector3> polygonPoints, double roundingRadius)
        {
            var roundedPolygon = new List<Vector3>();
            for (int i = 0; i < polygonPoints.Count(); i++)
            {
                int startIndex = (i == 0) ? polygonPoints.Count() - 1 : i - 1;
                int endIndex = (i == polygonPoints.Count() - 1) ? 0 : i + 1;

                roundedPolygon.AddRange(Round(polygonPoints[startIndex], polygonPoints[i], polygonPoints[endIndex], roundingRadius));
            }
            return roundedPolygon;
        }

        private static List<Vector3> Round(Vector3 startPoint, Vector3 cornerPoint, Vector3 endPoint, double roundingRadius)
        {
            var line1 = startPoint - cornerPoint;
            var line2 = endPoint - cornerPoint;

            return CornerPoints(line1, line2, cornerPoint, roundingRadius);
        }

        public static List<Vector3> CornerPoints(Vector3 line1, Vector3 line2, Vector3 cornerPoint, double radius)
        {
            var circleCenter = CircleUtilities.CircleCenterPoint(line1, line2, radius, cornerPoint);
            var (point1, point2) = CircleUtilities.CircleTangentPoints(line1, line2, radius, cornerPoint);
            var normal = Vector3.CrossProduct(point1 - circleCenter, 
                                              point2 - circleCenter);
            var angle = Vector3.AngleBetween(point1 - circleCenter, 
                                             point2 - circleCenter);
            angle *= 180 / Math.PI;
            var increment = angle / CornerSections;
            double currentAngle = 0;
            var points = new List<Vector3>() { };
            while(currentAngle <= angle)
            {
                var point = point1;
                point.Rotate(normal.Normalized(), currentAngle, circleCenter);
                points.Add(point);
                currentAngle += increment;
            }
            return points;
        }
    }

    public static class CircleUtilities
    {
        public static Vector3 CircleCenterPoint(Vector3 line1, Vector3 line2, double radius, Vector3 cornerPoint)
        {
            var distance = CornerToCenterDistance(Vector3.AngleBetween(line1, line2), radius);
            var vectorToCenter = (line1.Normalized() + line2.Normalized()).Normalized();

            return vectorToCenter * distance + cornerPoint;
        }

        public static (Vector3 point1, Vector3 point2) CircleTangentPoints(Vector3 line1, Vector3 line2, double radius, Vector3 cornerPoint)
        {
            var distance = CornerToTangentDistance(Vector3.AngleBetween(line1, line2), radius);
            var point1 = line1.Normalized() * distance + cornerPoint;
            var point2 = line2.Normalized() * distance + cornerPoint;

            return (point1, point2);
        }

        private static double CornerToCenterDistance(double angle, double radius)
        {
            return radius / Math.Sin(angle / 2);
        }

        private static double CornerToTangentDistance(double angle, double radius)
        {
            return radius / Math.Tan(angle / 2);
        }
    }
}
