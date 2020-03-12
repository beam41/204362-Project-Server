﻿using MheanMaa.Models;
using MheanMaa.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MheanMaa.Services
{
    public class DogService
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

        public Dog Get(string id) =>
            _dog.Find<Dog>(dog => dog.Id == id).FirstOrDefault();
        public Dog Create(Dog newDog)
        {
            _dog.InsertOne(newDog);
            return newDog;
        }
        public void Update(string id, Dog dogIn) =>
            _dog.ReplaceOne(dog => dog.Id == id, dogIn);
        public void Remove(Dog dogIn) =>
            _dog.DeleteOne(dog => dog.Id == dogIn.Id);
        public void Remove(string id) =>
            _dog.DeleteOne(dog => dog.Id == id);
    }
}