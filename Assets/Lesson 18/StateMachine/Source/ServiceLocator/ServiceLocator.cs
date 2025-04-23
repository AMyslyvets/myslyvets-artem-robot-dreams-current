using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineSystem.ServiceLocatorSystem
{
    /// <summary>
    /// Class that is created before any scene, holds different services, systems and managers
    /// Allowing single access point
    /// </summary>
    [DefaultExecutionOrder(-30)]
    public class ServiceLocator : MonoBehaviour
    {
        /// <summary>
        /// Method that is called by engine on game start, before any scene load
        /// due to RuntimeInitializeOnLoadMethod Attribute
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeServiceLocator()
        {
            // Create a new gameobject, add instance of ServiceLocator (this method, InitializeServiceLocator,
            // is a static method, it can be executed without any instance of class whatsoever)
            GameObject serviceLocator = new GameObject("ServiceLocator");
            Instance = serviceLocator.AddComponent<ServiceLocator>();
            // Marking gameobject DontDestroyOnLoad will make it persistant between scenes
            // It will be destroyed only on application end, or with manual Destroy call
            DontDestroyOnLoad(serviceLocator);
        }
        
        /// <summary>
        /// Standard singleton access static field
        /// </summary>
        public static ServiceLocator Instance { get; private set; }

        /// <summary>
        /// Collection of abstract services, key is a type service wants to be represented as
        /// </summary>
        private Dictionary<Type, IService> _services = new();
        
        /// <summary>
        /// Add a service to collection
        /// Declared type in field Type will be used as key
        /// </summary>
        /// <param name="service"></param>
        public void AddService(IService service)
        {
            _services.Add(service.Type, service);
        }

        /// <summary>
        /// Add a service to collection, but using custom type as key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="service"></param>
        public void AddServiceExplicit(Type type, IService service)
        {
            _services.Add(type, service);
        }
        
        /// <summary>
        /// Remove service by removing entry with key, declared in type field of service
        /// </summary>
        /// <param name="service"></param>
        public void RemoveService(IService service)
        {
            _services.Remove(service.Type);
        }
        
        /// <summary>
        /// Remove explicitly added service
        /// </summary>
        /// <param name="type"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public bool RemoveServiceExplicit(Type type, IService service)
        {
            // Double check that exactly this service is added under give key
            if (_services.TryGetValue(type, out IService serviceValue) && serviceValue == service)
            {
                return _services.Remove(type);
            }
            return false;
        }

        /// <summary>
        /// Given type, tries to return service registered under such type
        /// Service will be returned as type requested
        /// In case of absence of service or type mismatch, null will be returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() where T : class, IService
        {
            _services.TryGetValue(typeof(T), out IService service);
            return service as T;
        }
    }
}