using UnityEngine;

namespace Navigation2D.Geometry
{
    public static class GeometryHelper2D
    {
        #region Methods

        #region bool 
        /// <summary>
        /// Check if two segement intersect
        /// </summary>
        /// <param name="L1_start">start of the first segment</param>
        /// <param name="L1_end">end of the first segment</param>
        /// <param name="L2_start">start of the second segment</param>
        /// <param name="L2_end">end of the second segment</param>
        /// <returns>return true if segements intersect</returns>
        public static bool IsIntersecting(Vector2 _a, Vector2 _b, Vector2 _c, Vector2 _d, bool _includePoints = true)
        {
            float _denominator = (_d.y - _c.y) * (_b.x - _a.x) - (_d.x - _c.x) * (_b.y - _a.y);


            if (_denominator != 0.0f)
            {
                float _m = ((_d.x - _c.x) * (_a.y - _c.y) - (_d.y - _c.y) * (_a.x - _c.x)) / _denominator;

                float _k = ((_b.x - _a.x) * (_a.y - _c.y) - (_b.y - _a.y) * (_a.x - _c.x)) / _denominator;

                _m = (float)System.Math.Round(_m, 6);
                _k = (float)System.Math.Round(_k, 6);
                if (_includePoints)
                {
                    return (_m >= 0 && _m <= 1 && _k >= 0 && _k <= 1);
                }
                else
                {
                    return (_m > 0 && _m < 1 && _k > 0 && _k < 1);
                }
            }
            return false;
        }

        /// <summary>
        /// Check if two segement intersect
        /// </summary>
        /// <param name="L1_start">start of the first segment</param>
        /// <param name="L1_end">end of the first segment</param>
        /// <param name="L2_start">start of the second segment</param>
        /// <param name="L2_end">end of the second segment</param>
        /// <returns>return true if segements intersect</returns>
        public static bool IsIntersecting(Vector2 _a, Vector2 _b, Vector2 _c, Vector2 _d, out Vector2 _intersection, bool _includePoints = true)
        {
            _intersection = Vector2.zero;
            float _denominator = (_d.y - _c.y) * (_b.x - _a.x) - (_d.x - _c.x) * (_b.y - _a.y);


            if (_denominator != 0)
            {
                float _m = ((_d.x - _c.x) * (_a.y - _c.y) - (_d.y - _c.y) * (_a.x - _c.x)) / _denominator;

                float _k = ((_b.x - _a.x) * (_a.y - _c.y) - (_b.y - _a.y) * (_a.x - _c.x)) / _denominator;

                _m = (float)System.Math.Round(_m, 6);
                _k = (float)System.Math.Round(_k, 6);

                if (_includePoints && (_m >= 0 && _m <= 1 && _k >= 0 && _k <= 1))
                {
                    _intersection = _a + _m * (_b - _a);
                    return true;
                }
                else if (!_includePoints && (_m > 0 && _m < 1 && _k > 0 && _k < 1))
                {
                    _intersection = _a + _m * (_b - _a);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if a point is between two endpoints of a segment
        /// </summary>
        /// <param name="_firstSegmentPoint">First endpoint of the segment</param>
        /// <param name="_secondSegmentPoint">Second endpoint of the segment</param>
        /// <param name="_comparedPoint">Compared point</param>
        /// <returns></returns>
        public static bool PointContainedInSegment(Vector2 _firstSegmentPoint, Vector2 _secondSegmentPoint, Vector2 _comparedPoint)
        {
            Vector2 _ab = _secondSegmentPoint - _firstSegmentPoint;
            Vector2 _ac = _comparedPoint - _firstSegmentPoint;
            if (!IsColinear(_ab, _ac)) // Check Point Alignement
                return false;
            if (Vector2.Dot(_ab, _ac) < 0) // Check if opposite directions
                return false;
            if (Vector2.Dot(_ab, _ac) > _ab.sqrMagnitude) // Check if the point is between the two ends of the segment
                return false;

            return true;
        }

        /// <summary>
        /// Are the Vectors <see cref="_ab"/> and <see cref="_ac"/> colinear?
        /// </summary>
        /// <param name="_ab"></param>
        /// <param name="_ac"></param>
        /// <returns></returns>
        public static bool IsColinear(Vector2 _ab, Vector2 _ac)
        {
            // AB.x * AC.y - AC.x * AB.y
            return _ab.x * _ac.y - _ac.x * _ab.y == 0;
        }

        /// <summary>
        /// Is <see cref="_point"/> contained in the triangle [<see cref="_vertex1"/>, <see cref="_vertex2"/>, <see cref="_vertex3"/>]?
        /// </summary>
        /// <param name="_vertex1"></param>
        /// <param name="_vertex2"></param>
        /// <param name="_vertex3"></param>
        /// <param name="_point"></param>
        /// <returns></returns>
        public static bool InInsideTriangle(Vector2 _vertex1, Vector2 _vertex2, Vector2 _vertex3, Vector2 _point)
        {
            //Based on Barycentric coordinates
            float denominator = ((_vertex2.y - _vertex3.y) * (_vertex1.x - _vertex3.x) + (_vertex3.x - _vertex2.x) * (_vertex1.y - _vertex3.y));

            float a = ((_vertex2.y - _vertex3.y) * (_point.x - _vertex3.x) + (_vertex3.x - _vertex2.x) * (_point.y - _vertex3.y)) / denominator;
            float b = ((_vertex3.y - _vertex1.y) * (_point.x - _vertex3.x) + (_vertex1.x - _vertex3.x) * (_point.y - _vertex3.y)) / denominator;
            float c = 1 - a - b;

            return (a >= 0.0f && a <= 1.0f && b >= 0.0f && b <= 1.0f && c >= 0.0f && c <= 1.0f);
        }

        /// <summary>
        /// Is the point <see cref="_point"/> contained in the Circumscribed circle of the triangle [<see cref="_vertex1"/>, <see cref="_vertex2"/>, <see cref="_vertex3"/>]?
        /// </summary>
        /// <param name="_aVec"></param>
        /// <param name="_bVec"></param>
        /// <param name="_cVec"></param>
        /// <param name="_point"></param>
        /// <returns></returns>
        public static bool IsPointInsideCircle(Vector2 _aVec, Vector2 _bVec, Vector2 _cVec, Vector2 _point)
        {

            //This first part will simplify how we calculate the determinant
            float a = _aVec.x - _point.x;
            float d = _bVec.x - _point.x;
            float g = _cVec.x - _point.x;

            float b = _aVec.y - _point.y;
            float e = _bVec.y - _point.y;
            float h = _cVec.y - _point.y;

            float c = a * a + b * b;
            float f = d * d + e * e;
            float i = g * g + h * h;

            float determinant = (a * e * i) + (b * f * g) + (c * d * h) - (g * e * c) - (h * f * a) - (i * d * b);

            return determinant > 0;
        }

        /// <summary>
        /// Are the points of the triangles oriented clockwise? 
        /// </summary>
        /// <param name="_v1"></param>
        /// <param name="_v2"></param>
        /// <param name="_v3"></param>
        /// <returns></returns>
        public static bool IsTriangleClockwise(Vector2 _v1, Vector2 _v2, Vector2 _v3)
        {
            return _v1.x * _v2.y + _v3.x * _v1.y + _v2.x * _v3.y - _v1.x * _v3.y - _v3.x * _v2.y - _v2.x * _v1.y < 0;
        }
        #endregion

        #region float 
        /// <summary>
        /// if == 0, the vectors are colinear so the point is on the segment
        /// if > 0, the point is to the right of the vector 
        /// if < 0, the point is to the left of the vector 
        /// </summary>
        /// <param name="_start">Start Point</param>
        /// <param name="_end">End Point</param>
        /// <param name="_point">Point to compare</param>
        /// <returns></returns>
        public static float GetPointSideOnSegment(Vector2 _start, Vector2 _end, Vector2 _point, bool _includePointBehind = true)
        {
            if (!_includePointBehind)
            {
                if (Vector2.Dot((_end - _start).normalized, (_point - _start).normalized) <= -.5f)
                    return 0;
            }
            return (_point.x - _start.x) * (_end.y - _point.y) - (_point.y - _start.y) * (_end.x - _point.x);
        }

        /// <summary>
        /// Get the counter clockwise angle between the two Vectors 
        /// </summary>
        /// <param name="_referencePoint"></param>
        /// <param name="_firstPoint"></param>
        /// <param name="_secondPoint"></param>
        /// <returns></returns>
        public static float CounterClockwiseAngleBetween(Vector2 _referencePoint, Vector2 _firstPoint, Vector2 _secondPoint)
        {
            Vector2 _ab = _firstPoint - _referencePoint;
            Vector2 _ac = _secondPoint - _referencePoint;
            float _angle = -(Mathf.Atan2(_ab.y, _ab.x) - Mathf.Atan2(_ac.y, _ac.x)) * Mathf.Rad2Deg;
            if (_angle < 0) _angle += 360;
            return _angle;
        }

        /// <summary>
        /// Get the clockwise angle between the two Vectors 
        /// </summary>
        /// <param name="_referencePoint"></param>
        /// <param name="_firstPoint"></param>
        /// <param name="_secondPoint"></param>
        /// <returns></returns>
        public static float ClockwiseAngleBetween(Vector2 _referencePoint, Vector2 _firstPoint, Vector2 _secondPoint)
        {
            Vector2 _ab = _firstPoint - _referencePoint;
            Vector2 _ac = _secondPoint - _referencePoint;
            float _angle = (Mathf.Atan2(_ab.y, _ab.x) - Mathf.Atan2(_ac.y, _ac.x)) * Mathf.Rad2Deg;
            if (_angle < 0) _angle += 360;
            return _angle;
        }
        #endregion

        #region Vector2
        /// <summary>
        /// Get the transposed point of the predicted position on a segement between the previous and the next position
        /// Check if the targeted point is on the segment between the previous and the next points
        /// If it doesn't the normal point become the _nextPosition
        /// </summary>
        /// <param name="_predictedPosition">Predicted Position</param>
        /// <param name="_previousPosition">Previous Position</param>
        /// <param name="_nextPosition">Next Position</param>
        /// <returns></returns>
        public static Vector2 GetNormalPoint(Vector2 _predictedPosition, Vector2 _previousPosition, Vector2 _nextPosition)
        {
            Vector2 _ap = _predictedPosition - _previousPosition;
            Vector2 _ab = (_nextPosition - _previousPosition).normalized;
            Vector2 _ah = _ab * (Vector2.Dot(_ap, _ab));
            Vector2 _normal = (_previousPosition + _ah);
            Vector2 _min = Vector2.Min(_previousPosition, _nextPosition);
            Vector2 _max = Vector2.Max(_previousPosition, _nextPosition);
            if (_normal.x < _min.x || _normal.y < _min.y || _normal.x > _max.x || _normal.y > _max.y)
            {
                return _nextPosition;
            }
            return _normal;
        }
        #endregion

        #endregion
    }
}