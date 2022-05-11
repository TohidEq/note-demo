using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace NoteDemo
{
    internal class DBController
    {
        private MongoClient dbClient;
        private IMongoDatabase db;  // = dbClient.GetDatabase("dbname")



        /// <summary>
        /// connect to [mongodb://localhost:27017] default mongodb address
        /// </summary>
        public DBController()
        {
            dbClient = new MongoClient("mongodb://localhost:27017/");
            db = dbClient.GetDatabase("notedemo");
        }

        /// <summary>
        /// connect to custom db
        /// </summary>
        /// <param name="connection">mongodb address like: [mongodb://localhost:27017]</param>
        /// <param name="dbName">mongodb DBName</param>
        public DBController(string connection, string dbName)
        {
            dbClient = new MongoClient(connection);
            db = dbClient.GetDatabase(dbName);
        }



        /// <summary>
        /// get all types(_id, name, note_id) sort by name
        /// </summary>
        /// <returns>List<BsonDocument></returns>
        public List<BsonDocument> GetTypeSortByName()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("name");

            var types = db.GetCollection<BsonDocument>("Type");
            return types.Find(new BsonDocument()).Sort(sort).ToList(); ;
        }



        public List<BsonDocument> GetNotesSortById()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("_id");

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(new BsonDocument()).Sort(sort).ToList(); ;
        }

        public List<BsonDocument> GetNotesSortByTitle()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("title");

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(new BsonDocument()).Sort(sort).ToList(); ;
        }

        public List<BsonDocument> GetNotesSortByText()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("text");

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(new BsonDocument()).Sort(sort).ToList(); ;
        }
        public List<BsonDocument> GetNoteById(Int64 noteId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", noteId);

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(filter).ToList();
        }



        //just for test
        public List<BsonDocument> DD()
        {

            db = dbClient.GetDatabase("notedemo");
            var sort = Builders<BsonDocument>.Sort.Ascending("name");

            var type = db.GetCollection<BsonDocument>("Type");
            return type.Find(new BsonDocument()).Sort(sort).ToList();
        }













    }
}
