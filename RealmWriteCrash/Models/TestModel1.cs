using Realms;

namespace RealmWriteCrash.Models
{
    public class TestModel1 : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        
        public string Property1 { get; set; }
        
        public string Property2 { get; set; }
        
        public string Property3 { get; set; }
    }
}