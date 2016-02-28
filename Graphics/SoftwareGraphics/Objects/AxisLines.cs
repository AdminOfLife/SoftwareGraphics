using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SoftwareGraphics.Objects
{
    public class AxisLines : ModelObject
    {
        public AxisLines(float axisWidth, float axisLength, float arrowHeight)
        {
            float halfWidth = axisWidth / 2;
            float doubleWidth = axisWidth * 2;

            var xAxis = ModelBuilder.CreateParallelepiped(
                new Vector3(axisWidth, halfWidth, halfWidth),
                new Vector3(axisLength, 0, 0),
                new Vector3(0, -axisWidth, 0),
                new Vector3(0, 0, -axisWidth),
                Color.Red).Concat(
            ModelBuilder.CreatePyramid(
                new Vector3(axisWidth + axisLength + arrowHeight, 0, 0),
                arrowHeight,
                new Vector3(0, -doubleWidth, 0),
                new Vector3(0, 0, -doubleWidth),
                Color.Red));

            var yAxis = ModelBuilder.CreateParallelepiped(
                new Vector3(-halfWidth, axisWidth, halfWidth),
                new Vector3(0, axisLength, 0),
                new Vector3(axisWidth, 0, 0),
                new Vector3(0, 0, -axisWidth),
                Color.Green).Concat(
            ModelBuilder.CreatePyramid(
                new Vector3(0, axisWidth + axisLength + arrowHeight, 0),
                arrowHeight,
                new Vector3(doubleWidth, 0, 0),
                new Vector3(0, 0, -doubleWidth),
                Color.Green));

            var zAxis = ModelBuilder.CreateParallelepiped(
                new Vector3(-halfWidth, halfWidth, axisWidth),
                new Vector3(0, 0, axisLength),
                new Vector3(0, -axisWidth, 0),
                new Vector3(axisWidth, 0, 0),
                Color.Blue).Concat(
            ModelBuilder.CreatePyramid(
                new Vector3(0, 0, axisWidth + axisLength + arrowHeight),
                arrowHeight,
                new Vector3(doubleWidth, 0, 0),
                new Vector3(0, doubleWidth, 0),
                Color.Blue));

            var center = ModelBuilder.CreateParallelepiped(
                new Vector3(-axisWidth, axisWidth, axisWidth),
                new Vector3(doubleWidth, 0, 0),
                new Vector3(0, -doubleWidth, 0),
                new Vector3(0, 0, -doubleWidth),
                Color.Yellow);

            var triangles = xAxis.Concat(yAxis).Concat(zAxis).Concat(center);
            polygons.AddRange(triangles);
        }
    }
}
