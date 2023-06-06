﻿using System.Collections.Generic;
using lab3.Place;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTests
{
    [TestClass]
    public class PlaceRepositoryTests
    {
        private PlaceRepository _repository;
        private PlaceEntity _moscow;
        private PlaceEntity _voronezh;
        private PlaceEntity _new_york;
        private PlaceEntity _la;
        
        [TestInitialize]
        public void SetUp()
        {
            List<PlaceEntity> cities = new List<PlaceEntity>();
            _moscow = new PlaceEntity("Moscow", 300, 14);
            _voronezh = new PlaceEntity("Voronezh", 100, 3);
            _new_york = new PlaceEntity("New York", 1000, 24);
            _la = new PlaceEntity("LA", 500, 20);
            cities.Add(_moscow);
            cities.Add(_voronezh);
            cities.Add(_new_york);
            cities.Add(_la);
            _repository = new PlaceRepository(cities);
        }
        
        [TestMethod]
        public void SortingByPopulationTest()
        {
            _repository.SortDataByPopulation();
            Assert.AreEqual(_voronezh, _repository.Place[0]);
        }
        
        [TestMethod]
        public void SortingByPopulationDescendingTest()
        {
            _repository.SortDataByPopulationDescending();
            Assert.AreEqual(_new_york, _repository.Place[0]);
        }
        
        [TestMethod]
        public void SortingBySquareTest()
        {
            _repository.SortDataBySquare();
            Assert.AreEqual(_voronezh, _repository.Place[0]);
        }
        
        [TestMethod]
        public void SortingBySquareDescendingTest()
        {
            _repository.SortDataBySquareDescending();
            Assert.AreEqual(_new_york, _repository.Place[0]);
        }

        [TestMethod]
        public void AddObjectTest()
        {
            PlaceEntity dubai = new PlaceEntity("Dubai", 70000, 500);
            _repository.AddObject(dubai);
            Assert.IsTrue(_repository.Place.Contains(dubai));   
        }

        [TestMethod]
        public void DeleteObjectTest()
        {
            Assert.IsTrue(_repository.Place.Contains(_moscow));
            _repository.DeleteObject(0);
            Assert.IsFalse(_repository.Place.Contains(_moscow));
        }
    
        [TestMethod]
        public void FilterDataByPopulation()
        {
            List<PlaceEntity> cities = _repository.FilterDataByPopulation(300);
            Assert.IsFalse(cities.Contains(_voronezh));
            Assert.IsTrue(cities.Contains(_moscow));
        }
        
        [TestMethod]
        public void FilterDataBySquare()
        {
            List<PlaceEntity> cities = _repository.FilterDataBySquare(14);
            Assert.IsFalse(cities.Contains(_voronezh));
            Assert.IsTrue(cities.Contains(_moscow));
        }
        
    }
}
