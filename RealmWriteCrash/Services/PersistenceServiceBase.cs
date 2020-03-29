﻿using System;
 using System.Collections.Concurrent;
 using System.Threading;
 using Realms;

 namespace RealmWriteCrash.Services
{
   public abstract class PersistenceServiceBase
   {
      protected abstract RealmConfiguration RealmConfiguration { get; }

      protected abstract ConcurrentDictionary<int, Realm> RealmPool { get; }

      protected abstract object DictionaryLock { get; }
      
      Realm CreateInstance() => Realm.GetInstance(RealmConfiguration);

      /* approach #1
       
      Realm GetInstance()
      {
         return CreateInstance();
      }
      */
      
      Realm GetInstance()
      {
         var threadId = Thread.CurrentThread.ManagedThreadId;

         lock (DictionaryLock)
         {
            try
            {
               if (!RealmPool.TryGetValue(threadId, out var realm))
               {
                  realm = CreateInstance();
                  RealmPool.TryAdd(threadId, realm);                  
               }
               else if(realm.IsClosed)
               {
                  var newRealm = CreateInstance();
                  RealmPool.TryUpdate(threadId, newRealm, realm);
                  realm = newRealm;
               }
               else
               {
                  realm.Refresh();
               }

               return realm;
            }
            catch (Exception e)
            {
               System.Diagnostics.Debug.WriteLine($"realm exception: {e.Message}", e);
            }
         }

         return null;
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
      
      /*
      // makes only sense with approach #1 
      public bool Exec(Action<Realm> action)
      {

         try
         {
            using (var instance = GetInstance())
            {
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
         }
         catch (Exception e)
         {
            System.Diagnostics.Debug.WriteLine($"realm exception: {e.Message}", e);
         }

         return false;
      }

      // makes only sense with approach #1
      public T Exec<T>(Func<Realm, T> action)
      {

         try
         {
            using (var instance = GetInstance())
            {
               if (instance != null)
               {
                  return action(instance);
               }
               else
               {
                  System.Diagnostics.Debug.WriteLine("realm exception: realm is null");
               }
            }
         }
         catch (Exception e)
         {
            System.Diagnostics.Debug.WriteLine($"realm exception: {e.Message}", e);
         }

         return default;
      }
      */
   }
}
