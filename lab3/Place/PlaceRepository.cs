﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using lab3.FileWork.JSON;
using lab3.FileWork.Services;
using lab3.FileWork.XML;
using Newtonsoft.Json;
using ServiceStack.Text;

namespace lab3.Place
{
    public class PlaceRepository : IRepository<PlaceEntity>
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        
        private List<PlaceEntity> _place;

        public List<PlaceEntity> Place => _place;

        public PlaceRepository()
        {
            _place = new List<PlaceEntity>();
        }
        
        public PlaceRepository(List<PlaceEntity> place)
        {
            _place = place;
        }
        
        public PlaceRepository(string filePath, string extension)
        {
            IReader<PlaceEntity> reader;
            switch (extension)
            {
                case string a when a.Contains("json"):
                    reader = new JsonReaderFile<PlaceEntity>();
                    _place = reader.GetData(filePath);
                    break;
                case string a when a.Contains("xml"):
                    reader = new XMLReader<PlaceEntity>();
                    _place = reader.GetData(filePath);
                    break;
                case string a when a.Contains("csv"):
                    RepositoryFromCsv(filePath);
                    break;
            }
        }
        
        private void RepositoryFromCsv(string filePath)
        {
            var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ","
            };
            StreamReader reader = new StreamReader(filePath);
            _place = new List<PlaceEntity>();
            using (var csvReader = new CsvHelper.CsvReader(reader, csvConfig))
            {
                csvReader.Read();
                while (csvReader.Read())
                {
                    csvReader.TryGetField<string>(0, out var currentName);
                    csvReader.TryGetField<string>(1, out var currentPopulationStr);
                    csvReader.TryGetField<string>(2, out var currentSquareStr);
                    var currentPopulation = int.Parse(currentPopulationStr);
                    var currentSquare = int.Parse(currentSquareStr);
                    _place.Add(new PlaceEntity(currentName, currentPopulation, currentSquare));
                }
            }
            reader.Close();
        }

        public void SortDataByPopulation()
        {
            _place = _place.OrderBy(o => o.Population).ToList();
            Log.Info("PlaceRepository: Sorted data by population");
        }

        public void SortDataByPopulationDescending()
        {
            _place = _place.OrderByDescending(o => o.Population).ToList();
            Log.Info("PlaceRepository: Sorted data by population (descending)");
        }

        public void SortDataBySquare()
        {
            _place = _place.OrderBy(o => o.Square).ToList();
            Log.Info("PlaceRepository: Sorted data by square");
        }

        public void SortDataBySquareDescending()
        {
            _place = _place.OrderByDescending(o => o.Square).ToList();
            Log.Info("PlaceRepository: Sorted data by square (descending)");
        }

        public void AddObject(PlaceEntity obj)
        {
            _place.Add(obj);
            Log.Info("PlaceRepository: Added place with " + obj);
        }

        public void AddObject(string name, int population, int square)
        {
            PlaceEntity obj = new PlaceEntity(name, population, square);
            _place.Add(obj);
            Log.Info("PlaceRepository: Added place with " + obj);
        }

        public void DeleteObject(int id)
        {
            Log.Info("PlaceRepository: Removed place with " + _place[id]);
            _place.RemoveAt(id);
        }

        public List<PlaceEntity> FilterDataByPopulation(int population)
        {
            Log.Info("PlaceRepository: Filtered data by population >= " + population);
            return _place.Where(place => place.Population >= population).ToList();
        }

        public List<PlaceEntity> FilterDataBySquare(int square)
        {
            Log.Info("PlaceRepository: Filtered data by square >= " + square);
            return _place.Where(place => place.Square >= square).ToList();
        }

        public List<PlaceEntity> GetData()
        {
            return Place;
        }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(_place);
        }

        public string ToCsv()
        {
            return CsvSerializer.SerializeToCsv(_place);
        }

        public string ToXml()
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System
                .Xml.Serialization.XmlSerializer(_place.GetType());
            using(StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, _place);
                return textWriter.ToString();
            }
        }
    }
}