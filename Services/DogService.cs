﻿using MheanMaa.Models;
using MheanMaa.Services.Interfaces;
using MheanMaa.Settings;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace MheanMaa.Services
{
    public class DogService : IMongoServiceBaseExtended<Dog>
    {
        private readonly IMongoCollection<Dog> _dog;
        public DogService(IDBSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DBName);
            _dog = database.GetCollection<Dog>(settings.DogsColName);
        }

        public List<Dog> Get() =>
            _dog.Find(_ => true).ToList();

        public List<Dog> Get(int deptNo) =>
            _dog.Find(dog => dog.DeptNo == deptNo).ToList();

        public Dog Get(string id, int deptNo) =>
            _dog.Find(dog => dog.Id == id && dog.DeptNo == deptNo).FirstOrDefault();

        public Dog Get(string id) =>
            _dog.Find(dog => dog.Id == id).FirstOrDefault();

        public void Create(Dog newDog) => _dog.InsertOne(newDog);

        public void Update(string id, Dog dogIn) =>
            _dog.ReplaceOne(dog => dog.Id == id, dogIn);

        public void Remove(Dog dogIn) =>
            _dog.DeleteOne(dog => dog.Id == dogIn.Id);
    }
}
