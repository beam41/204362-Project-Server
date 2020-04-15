using MheanMaa.Models;
using MheanMaa.Settings;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MheanMaa.Services
{
    public class ReportService
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

        public List<Report> Get(int deptNo) =>
            _reports.Find(rep => rep.DeptNo == deptNo).ToList();

        public Report Get(string id, int deptNo) =>
            _reports.Find(rep => rep.Id == id && rep.DeptNo == deptNo).FirstOrDefault();

        public Report Get(string id) =>
            _reports.Find(rep => rep.Id == id).FirstOrDefault();

        public List<Report> GetAcceptedReports() =>
            _reports.Find(rep => rep.Accepted).ToList();

        public Report Create(Report newRep)
        {
            _reports.InsertOne(newRep);
            return newRep;
        }

        public void Update(string id, Report repIn) =>
            _reports.ReplaceOne(rep => rep.Id == id, repIn);

        public void Remove(Report repIn) =>
            _reports.DeleteOne(rep => rep.Id == repIn.Id);

        public void Remove(string id) =>
            _reports.DeleteOne(rep => rep.Id == id);
    }
}