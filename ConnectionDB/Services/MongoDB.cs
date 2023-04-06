using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
//using Newtonsoft.Json;

namespace ConnectionDB.Model
{
    public class MongoDB
    {
        private MongoClient Connection = null;
        //private const string DBname = "emujDB";
        private const string DBname = "emujDB";

        private MongoClient MongoConnection(ref string Salah)
        {

            try
            {
                var MangoSetting = new MongoClientSettings
                {
                    Servers = new[]
                    {
                      new MongoServerAddress("12.1.1.55", 27017),
                      new MongoServerAddress("12.1.1.56", 27017)
                    },
                    Credential = MongoCredential.CreateCredential(
                        //databaseName: DBname,
                        databaseName: "admin",
                        username: "adminsa",
                        password: "saktmb123!@#"
                    ),
                    ReplicaSetName = "rs0",
                    MinConnectionPoolSize = 1,
                    MaxConnectionPoolSize = 100,
                    //DirectConnection = false,
                    MaxConnectionIdleTime = TimeSpan.FromSeconds(2),
                    MaxConnectionLifeTime = TimeSpan.FromSeconds(3),
                    UseTls = false,
                    WriteConcern = new WriteConcern(WriteConcern.WValue.Parse("1"), wTimeout: TimeSpan.Parse("10"), journal: false)
                };

                try
                {
                    if (Connection == null)
                    {
                        Connection = new MongoClient(MangoSetting);
                    }
                }
                catch (Exception ex) { Salah = ex.Message.ToString(); }

            }
            catch (Exception ex)
            {
                Salah = ex.Message.ToString();
            }
            return Connection;
        }

        public bool InsertData(string Data, string ColName, ref string Salah)
        {
            try
            {
                MongoClient CreateConn = MongoConnection(ref Salah);
                var database = CreateConn.GetDatabase(DBname);

                IMongoCollection<BsonDocument> mongoCollection = database.GetCollection<BsonDocument>(ColName);
                var document = new BsonDocument();
                var obj = BsonDocument.Parse(Data);
                mongoCollection.InsertOne(obj);
                return true;
            }
            catch (Exception ex)
            {
                Salah = ex.Message.ToString();
                return false;
            }
        }

        public bool CreateCollection(string ColName, ref string Salah)
        {
            try
            {
                MongoClient CreateConn = MongoConnection(ref Salah);
                var database = CreateConn.GetDatabase(DBname);

                if (CollectionExistsAsync(ColName) == false) { database.CreateCollection(ColName); }
                return true;
            }
            catch (Exception ex)
            {
                Salah = ex.Message.ToString();
                return false;
            }
        }

        private bool CollectionExistsAsync(string collectionName)
        {
            string Salah = "";
            MongoClient CreateConn = MongoConnection(ref Salah);
            var database = CreateConn.GetDatabase(DBname);
            var filter = new BsonDocument("name", collectionName);
            var collections = database.ListCollectionNames(new ListCollectionNamesOptions { Filter = filter });
            return collections.Any();
        }

        public DataTable GetData(string ColectionID, FilterDefinition<BsonDocument> filter, ProjectionDefinition<BsonDocument> project, ref string Salah)
        {
            DataTable Data = new DataTable();
            try
            {

                MongoClient CreateConn = MongoConnection(ref Salah);
                var database = CreateConn.GetDatabase(DBname);
                IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(ColectionID);
                List<BsonDocument> documents = collection.Find(filter).Project(project).ToList();
                if (documents != null && documents.Count > 0)
                {
                    DataTable dt = ConvertBsonToDataTable(documents);
                    Data.Merge(dt);
                }
                return Data;
            }
            catch (Exception ex)
            {
                Salah = ex.Message.ToString();
                return Data;
            }
        }

        public void UpdateData(string ColectionID, FilterDefinition<BsonDocument> filter, string Parameter, string Value, ref string Salah)
        {
            try
            {
                MongoClient CreateConn = MongoConnection(ref Salah);
                var database = CreateConn.GetDatabase(DBname);
                var update = Builders<BsonDocument>.Update.Set(Parameter, Value);
                var collection = database.GetCollection<BsonDocument>(ColectionID);
                collection.UpdateOne(filter, update);
                Salah = "0";
            }
            catch (Exception ex)
            {
                Salah = ex.Message.ToString();

            }
        }

        private DataTable ConvertBsonToDataTable(List<BsonDocument> data)
        {
            if (data != null && data.Count > 0)
            {
                DataTable dt = new DataTable(data.ToString());
                foreach (BsonDocument doc in data)
                {
                    foreach (var elm in doc.Elements)
                    {
                        if (!dt.Columns.Contains(elm.Name)) { dt.Columns.Add(new DataColumn(elm.Name)); }
                    }
                    DataRow dr = dt.NewRow();
                    foreach (var elm in doc.Elements) { dr[elm.Name] = elm.Value; }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            else { return null; }
        }
    }
}
