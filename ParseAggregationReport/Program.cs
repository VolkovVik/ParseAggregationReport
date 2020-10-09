using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1;
using ParseReport.Model;

namespace ParseReport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 150;
            Console.Title = "Parse report";
            Console.ForegroundColor = ConsoleColor.Blue;

            CheckReportData();

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        private static void CheckDatabaseData()
        {
            const string databaseName = "aspu";
            var database = new MongoDb<BlockModel>(databaseName, "blocks");
            var blocks = database.Read();
            var database1 = new MongoDb<ProductModel>(databaseName, "products");
            var products = database1.Read();
        }

        private static void CheckReportData() {
            const string pathUse = "C:\\Users\\Vvolkov\\Desktop\\SmokyShaft\\1355_CreateUtilisationReport.json";
            const string pathAggregation = "C:\\Users\\Vvolkov\\Desktop\\SmokyShaft\\1356_CreateAggregationReport.json";

            var report = new Report();
            var aggregationProduct = report.GetAggregationProduct(pathAggregation).ToList();
            var useProduct = report.GetUseProduct(pathUse)
                                   .Select(p =>
                                        p.StartsWith("01") || p.StartsWith("02")
                                            ? p.Substring(0, 25)
                                            : p.Substring(0, 21))
                                   .ToList();

            var diffirent = aggregationProduct.Except(useProduct).ToList();
            var common = aggregationProduct.Intersect(useProduct).ToList();

            var useDuplicate = GetDuplicateCodes(useProduct).ToList();
            var aggregationDuplicate = GetDuplicateCodes(aggregationProduct).ToList();


            var parent = FindParent(report.GetAggregationData(pathAggregation), "04640112140537+npOcP<");
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

        private static IEnumerable<string> FindParent( AggregationReport data, string code ) =>
            data.aggregationUnits
                .Where( u => u.sntins.Any( code.StartsWith ) )
                .Select( u => u.unitSerialNumber )
                .ToList();
    }
}
