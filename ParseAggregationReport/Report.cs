using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ParseReport
{
    public class Report
    {
        private static string ReadFile(string path)
        {
            using var r = new StreamReader(path);
            return r.ReadToEnd();
        }

        public UseReport GetUseData(string path)
        {
            var str = ReadFile(path);
            var data = JsonConvert.DeserializeObject<UseReport>(str);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return data;
        }

        public IEnumerable<string> GetUseProduct(IEnumerable<string> paths)
        {
            var products = new List<string>();
            foreach (var list in paths.Select(GetUseProduct))
            {
                products.AddRange(list);
            }
            return products;
        }

        public IEnumerable<string> GetUseProduct(string path)
        {
            var str = ReadFile(path);
            var data = JsonConvert.DeserializeObject<UseReport>(str);
            return data.sntins;
        }

        public AggregationReport GetAggregationData(string path)
        {
            var str = ReadFile(path);
            var data = JsonConvert.DeserializeObject<AggregationReport>(str);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return data;
        }

        public IEnumerable<string> GetAggregationProduct(IEnumerable<string> paths)
        {
            var products = new List<string>();
            foreach (var list in paths.Select(GetAggregationProduct))
            {
                products.AddRange(list);
            }
            return products;
        }

        public IEnumerable<string> GetAggregationProduct(string path)
        {
            var str = ReadFile(path);
            var data = JsonConvert.DeserializeObject<AggregationReport>(str);

            var products = new List<string>();
            foreach (var item in data.aggregationUnits)
            {
                products.AddRange(item.sntins);
            }
            return products;
        }

        public IEnumerable<string> GetAggregationBlock(IEnumerable<string> paths)
        {
            var blocks = new List<string>();
            foreach (var list in paths.Select(GetAggregationBlock))
            {
                blocks.AddRange(list);
            }
            return blocks;
        }

        public IEnumerable<string> GetAggregationBlock(string path)
        {
            var str = ReadFile(path);
            var data = JsonConvert.DeserializeObject<AggregationReport>(str);
            return data.aggregationUnits.Select(item => item.unitSerialNumber).ToList();
        }
    }

    public class AggregationReport
    {
        public List<AggregationUnit> aggregationUnits {get; set;}
        public string participantId {get; set;}
        public int productionLineId {get; set;}
    }

    public class AggregationUnit
    {
        public int aggregatedItemsCount {get; set;}
        public string aggregationType {get; set;}
        public int aggregationUnitCapacity {get; set;}
        public List<string> sntins {get; set;}
        public string unitSerialNumber {get; set;}

    }

    public class UseReport
    {
        public int productionLineId {get; set;}
        public List<string> sntins {get; set;}
        public string usageType {get; set;}
    }
}