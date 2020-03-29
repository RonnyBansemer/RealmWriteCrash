using System.Collections.Concurrent;
using Realms;

namespace RealmWriteCrash.Services
{
   sealed class PersistenceService1 : PersistenceServiceBase
   {
      static readonly ConcurrentDictionary<int, Realm> _realmPool = new ConcurrentDictionary<int, Realm>();

      static readonly object _dictionaryLock = new object();
      
      static readonly RealmConfiguration RealmConfigurationStatic = new RealmConfiguration("realm1.realm")
      {
         ShouldCompactOnLaunch = (totalBytes, usedBytes) =>
         {
            var oneHundredMB = 100 * 1024 * 1024;

            return totalBytes > (ulong)oneHundredMB && (double)usedBytes / totalBytes < 0.5;
         }
      };

      protected override RealmConfiguration RealmConfiguration => RealmConfigurationStatic;
      
      protected override ConcurrentDictionary<int, Realm> RealmPool => _realmPool;

      protected override object DictionaryLock => _dictionaryLock;
   }
}
