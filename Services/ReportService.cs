using MheanMaa.Models;
using MheanMaa.Services.Interface;
using MheanMaa.Settings;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MheanMaa.Services
{
    public class ReportService : IMongoServiceBase<Report>
    {
        private readonly IMongoCollection<Report> _reports;

        public ReportService(IDBSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DBName);

            _reports = database.GetCollection<Report>(settings.ReportsColName);
        }

        public List<Report> Get() =>
            _reports.Find(_ => true).ToList();

        public Report Get(string id) =>
            _reports.Find(rep => rep.Id == id).FirstOrDefault();

        public void Create(Report newRep) => _reports.InsertOne(newRep);

        public void Update(string id, Report repIn) =>
            _reports.ReplaceOne(rep => rep.Id == id, repIn);

        public void Remove(Report repIn) =>
            _reports.DeleteOne(rep => rep.Id == repIn.Id);
    }
}