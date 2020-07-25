using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ParseAggregationReport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Parse aggregation report";
            Console.ForegroundColor = ConsoleColor.Blue;

            // d:\Project\Vekas\_gitHub\TestParseAggregationReport\5445.json

            Console.WriteLine("Enter path file:");
            var path = Console.ReadLine();
            if ( string.IsNullOrWhiteSpace( path ) ) {
                Console.WriteLine( "File path not set" );
                return;
            }
            if (!File.Exists(path))
            {
                Console.WriteLine("File not found");
                return;
            }

            using var r1 = new StreamReader(path);
            var str = r1.ReadToEnd();
            var report = JsonConvert.DeserializeObject<AggregationReport>(str);
            var json = JsonConvert.SerializeObject(report, Formatting.Indented);
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var item in report.aggregationUnits)
            {
                var unique = GetUniqueCodes( item.sntins).ToList();
                var duplicate=GetDuplicateCodes( item.sntins).ToList();
                if (duplicate.Any())
                {
                    Console.WriteLine( $"{item.unitSerialNumber}" );
                    Console.WriteLine($"{new string(' ', 4)}duplicate code - {duplicate.Count}");
                    foreach (var code in duplicate)
                        Console.WriteLine( $"{new string( ' ', 4 )}{code}");
                }
                var result = ContainsCode(item.sntins, "11111", "22222");
            }
            
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static IEnumerable<string> GetUniqueCodes(IEnumerable<string> codes)
        {
            var enumerable = codes.ToList();
            return enumerable.Distinct();
        }

        private static IEnumerable<string> GetDuplicateCodes(IEnumerable<string> codes)
        {
            var enumerable = codes.ToList();
            return enumerable
                  .GroupBy(code => code)
                  .Where(code => code.Count() > 1)
                  .Select(code => code.Key);
        }

        private static bool ContainsCode(IEnumerable<string> codes, params string[] findCodes)
        {
            var enumerable = codes.ToList();
            var result = enumerable.Intersect(findCodes);
            return result.Any();
        }
    }

    public class AggregationReport {
        public List<AggregationUnit> aggregationUnits { get; set; }
        public string participantId { get; set; }
        public int productionLineId { get; set; }

    }

    public class AggregationUnit {
        public int aggregatedItemsCount { get; set; }
        public string aggregationType { get; set; }
        public int aggregationUnitCapacity { get; set; }
        public List<string> sntins { get; set; }
        public string unitSerialNumber { get; set; }

    }
}
