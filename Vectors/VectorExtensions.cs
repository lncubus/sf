using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vectors
{
    public static class VectorExtensions
    {
        public static Vector3 GetXYZ(this Quaternion q)
        {
            return new Vector3
            {
                X = q.X, Y = q.Y, Z = q.Z
            };
        }

        /// <summary>
        /// https://blog.molecular-matters.com/2013/05/24/a-faster-quaternion-vector-multiplication/
        /// </summary>
        /// <param name="q"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 Rotate (this Quaternion q, Vector3 v)
        {
            if (q.IsIdentity)
                return v;
            Vector3 q_xyz = q.GetXYZ();
            Vector3 t = 2 * Vector3.Cross(q_xyz, v);
            Vector3 Cqt = Vector3.Cross(q_xyz, t);
            Vector3 vv = v + q.W * t + Cqt;
            return vv;
        }
    }
}
