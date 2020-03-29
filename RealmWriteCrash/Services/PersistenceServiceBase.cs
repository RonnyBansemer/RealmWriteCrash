﻿using System;
using Realms;

 namespace RealmWriteCrash.Services
{
   public abstract class PersistenceServiceBase
   {
      protected abstract RealmConfiguration RealmConfiguration { get; }
      
      Realm CreateInstance() => Realm.GetInstance(RealmConfiguration);

      Realm GetInstance()
      {
         return CreateInstance();
      }

      public bool Exec(Action<Realm> action)
      {

         try
         {
            var instance = GetInstance();

            if (instance != null)
            {
               action(instance);

               return true;
            }
            else
            {
               System.Diagnostics.Debug.WriteLine("realm exception: realm is null");
            }
         }
         catch (Exception e)
         {
            System.Diagnostics.Debug.WriteLine($"realm exception: {e.Message}", e);
         }

         return false;
      }

      public T Exec<T>(Func<Realm, T> action)
      {

         try
         {
            var instance = GetInstance();

            if (instance != null)
            {
               return action(instance);
            }
            else
            {
               System.Diagnostics.Debug.WriteLine("realm exception: realm is null");
            }
         }
         catch (Exception e)
         {
            System.Diagnostics.Debug.WriteLine($"realm exception: {e.Message}", e);
         }

         return default;
      }
   }
}
