using System.Collections.Concurrent;
using Realms;

namespace RealmWriteCrash.Services
{
   sealed class PersistenceService2 : PersistenceServiceBase
   {
      static readonly ConcurrentDictionary<int, Realm> _realmPool = new ConcurrentDictionary<int, Realm>();

      static readonly object _dictionaryLock = new object();
      
      static readonly RealmConfiguration RealmConfigurationStatic = new RealmConfiguration("realm2.realm")
      {
         EncryptionKey = new byte[64]
         {
            0xa4, 0xf7, 0xff, 0xd9, 0xe1, 0x34, 0x3e, 0x0a, 0x84, 0x0b, 0x41, 0x5f, 0xa9, 0x43, 0x65, 0xf5,
            0xe1, 0x5e, 0xca, 0xb9, 0x0e, 0x8b, 0xc3, 0xcb, 0x97, 0xee, 0xd7, 0xcf, 0xd4, 0x81, 0x0e, 0x79,
            0xf9, 0x39, 0xb4, 0x8a, 0xe4, 0x03, 0x3a, 0x76, 0xfc, 0xf7, 0xd9, 0x94, 0x50, 0x6f, 0x77, 0x87,
            0x58, 0xf3, 0x68, 0xcc, 0x9a, 0xec, 0x1d, 0x97, 0xcd, 0xea, 0x6a, 0x33, 0x7c, 0x72, 0x37, 0x5c
         },

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
