using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Project1.Modal
{
    public class Employee
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("employeeID")]
        public int EmployeeID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }
    }
}
