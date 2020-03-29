using Realms;

namespace RealmWriteCrash.Services
{
   sealed class PersistenceService1 : PersistenceServiceBase
   {
      static readonly RealmConfiguration RealmConfigurationStatic = new RealmConfiguration("realm1.realm")
      {
         ShouldCompactOnLaunch = (totalBytes, usedBytes) =>
         {
            var oneHundredMB = 100 * 1024 * 1024;

            return totalBytes > (ulong)oneHundredMB && (double)usedBytes / totalBytes < 0.5;
         }
      };

      protected override RealmConfiguration RealmConfiguration => RealmConfigurationStatic;
   }
}
