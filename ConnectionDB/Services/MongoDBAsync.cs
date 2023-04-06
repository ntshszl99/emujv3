using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;
using System.Linq;
//using BPCApi.Constructor;
using MongoDB.Bson;
using System.Reflection.Metadata;
//using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace ConnectionDB.Services
{
    public class MongoDBAsync
    {
        private static MongoClient Connection = null;
        private const string DBname = "emujDB";
        //private const string DBname = "emujDB";

        private static string MongoConnection()
        {
            string Salah = "";
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
            return Salah;
        }

        public static async Task<(DataTable data, int TotalPage, string Salah)> GetData(string ColectionID, FilterDefinition<BsonDocument> filter,
            ProjectionDefinition<BsonDocument> project, int CurrPage, int PageSize)
        {
            DataTable Data = new DataTable();
            string Salah = "";
            try
            {
                Salah = MongoConnection();
                var database = Connection.GetDatabase(DBname);
                IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(ColectionID);
                var result = await GetDataAsync(collection, filter, CurrPage, PageSize);
                return (result.data, result.totalPages, Salah);
            }
            catch (Exception ex)
            {
                Salah = ex.Message.ToString();
                return (Data, 0, Salah);
            }
        }

        private static async Task<(int totalPages, DataTable data)> GetDataAsync(
                IMongoCollection<BsonDocument> collection,
                FilterDefinition<BsonDocument> filterDefinition,
                //SortDefinition<MongoDB.Bson.BsonDocument> sortDefinition,
                int page,
                int pageSize)
        {
            DataTable Result = new DataTable();

            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<BsonDocument, AggregateCountResult>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Count<BsonDocument>()
                }));

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<BsonDocument, BsonDocument>.Create(
                    new PipelineStageDefinition<BsonDocument, BsonDocument>[]
                {
                //PipelineStageDefinitionBuilder.Sort(sortDefinition),
                PipelineStageDefinitionBuilder.Skip<BsonDocument>((page - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<BsonDocument>(pageSize),
                }));


            var aggregation = await collection.Aggregate()
                .Match(filterDefinition)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count;

            var totalPages = (int)Math.Ceiling((double)count / pageSize);

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<BsonDocument>().ToList();

            DataTable dt = ConvertBsonToDataTable(data);
            Result.Merge(dt);
            return (totalPages, Result);
        }

        private static DataTable ConvertBsonToDataTable(List<BsonDocument> data)
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
