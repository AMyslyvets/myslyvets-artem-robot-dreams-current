using System.Runtime.CompilerServices;
using UnityEngine;

namespace Animation
{
    public static class ColliderUtility
    {
        public static int GetManagedHasCode(this Collider collider)
        {
            int hashCode = RuntimeHelpers.GetHashCode(collider);
            return hashCode;
        }
    }
}