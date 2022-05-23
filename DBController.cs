﻿using System;
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




        //===================================//
        //= start INSERT and UPDATE METHODS =//
        //==...............................==//

        public Int64 InsertToNote(string title, string text, string type)
        {

            var sort = Builders<BsonDocument>.Sort.Ascending("name");

            var types = db.GetCollection<BsonDocument>("Type");
            //return types.Find(new BsonDocument()).Sort(sort).ToList(); ;
            return MakeId();
        }
        //===.............................===//
        //== end INSERT and UPDATE METHODS ==//
        //===================================//







        //==================================//
        //=== start GET and FIND METHODS ===//
        //====..........................====//

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
        public List<BsonDocument> GetNotesIdSortById()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("_id");

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(new BsonDocument()).Sort(sort).Project("{_id:1}").ToList(); ;
        }

        public List<BsonDocument> GetNotesSortByTitle()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("title");

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(new BsonDocument()).Sort(sort).ToList(); ;
        }

        

        public List<BsonDocument> GetNotesIdSortByTitle()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("title");

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(new BsonDocument()).Sort(sort).Project("{_id:1}").ToList(); ;
        }

        public List<BsonDocument> GetNotesSortByText()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("text");

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(new BsonDocument()).Sort(sort).ToList(); ;
        }

        public List<BsonDocument> GetNotesIdSortByText()
        {
            var sort = Builders<BsonDocument>.Sort.Ascending("text");

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(new BsonDocument()).Sort(sort).Project("{_id:1}").ToList(); ;
        }

        public List<BsonDocument> GetNoteById(string noteId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", noteId);

            var note = db.GetCollection<BsonDocument>("Note");
            return note.Find(filter).ToList();
        }

        public List<BsonDocument> GetTypesByNoteId(string noteId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("note_id", noteId.ToString());
            var types = db.GetCollection<BsonDocument>("TypeNote");
            return types.Find(filter).Project("{type:1}").ToList();
        }

        public List<BsonDocument> GetNoteIdByType(string type)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("type", type);

            var typeNote = db.GetCollection<BsonDocument>("TypeNote");
            return typeNote.Find(filter).Project("{note_id:1}").ToList();
        }
        public List<BsonDocument> GetNoteIdByNotThisType(string type)
        {


            var filter = Builders<BsonDocument>.Filter.Ne("type", type);
            var typeNote = db.GetCollection<BsonDocument>("TypeNote");
            return typeNote.Find(filter).Project("{note_id:1}").ToList();
        }
        public List<BsonDocument> GetNoteIdByNotThisTypes(string type1, string type2)
        {


            var filter = Builders<BsonDocument>.Filter.Ne("type", type1) &                             Builders<BsonDocument>.Filter.Ne("type", type2);
            var typeNote = db.GetCollection<BsonDocument>("TypeNote");
            return typeNote.Find(filter).Project("{note_id:1}").ToList();
        }

        //=====........................=====//
        //==== end GET and FIND METHODS ====//
        //==================================//



        //==================================//
        //==== Delete note and noteType ====//
        //=====........................=====//
        /// <summary>
        /// Delete note and noteType by note id
        /// </summary>
        /// <param name="noteId">note id int32</param>
        /// <returns>boolean True or False</returns>
        public bool DeleteByNoteId(int noteId)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("note_id", noteId);
                var typeNote = db.GetCollection<BsonDocument>("TypeNote");
                typeNote.DeleteMany(filter);

                filter = Builders<BsonDocument>.Filter.Eq("_id", noteId);
                var note = db.GetCollection<BsonDocument>("Note");
                note.DeleteMany(filter);

                return true;
            }catch (Exception)
            {
                return false;
            }
        }
        //=====........................=====//
        //== end Delete note and noteType ==//
        //==================================//








        //just for test
        public List<BsonDocument> DD()
        {

            db = dbClient.GetDatabase("notedemo");
            //var sort = Builders<BsonDocument>.Sort.Ascending("title");

            var type = db.GetCollection<BsonDocument>("Note");
            return type.Find(new BsonDocument()).Project("{_id: 0}").ToList();


            // to not get _id:
            // type.Find(new BsonDocument()).Project("{_id: 0}").ToList();
            //    <<   .Project("{_id: 0}")   >>
        }



        








        //======== custom methods =========//
        private int MakeId()
        {
            Random random = new Random();
            return Convert.ToInt32(
                DateTime.Now.ToString("hhmmss").
                ToString() + 
                random.Next(100,1000).
                ToString()
                );
        }


    }
}
