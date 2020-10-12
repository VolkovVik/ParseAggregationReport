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
            var pathUse = new []{
                "C:\\Users\\Vvolkov\\Desktop\\C95111020-9 [2020-10-11]\\reports111\\4313_CreateUtilisationReport.json",
                "C:\\Users\\Vvolkov\\Desktop\\C95111020-9 [2020-10-11]\\reports111\\4314_CreateUtilisationReport.json"};
            var pathAggregation = new []{
                "C:\\Users\\Vvolkov\\Desktop\\C95111020-9 [2020-10-11]\\reports111\\4315_CreateAggregationReport.json",
                "C:\\Users\\Vvolkov\\Desktop\\C95111020-9 [2020-10-11]\\reports111\\4316_CreateAggregationReport.json"};

            var report = new Report();
            var aggregationProduct = report.GetAggregationProduct(pathAggregation).ToList();
            var useProduct = report.GetUseProduct(pathUse)
                                   .Select(p =>
                                        p.StartsWith("01") || p.StartsWith("02")
                                            ? p.Substring(0, 25)
                                            : p.Substring(0, 21))
                                   .ToList();

            var countProduct = useProduct.Count(p => !p.StartsWith("01") && !p.StartsWith("02"));
            var countBlock = useProduct.Count(p => p.StartsWith("01") || p.StartsWith("02"));

            var different = aggregationProduct.Except(useProduct).ToList();
            var common = aggregationProduct.Intersect(useProduct).ToList();

            var useDuplicate = GetDuplicateCodes(useProduct).ToList();
            var aggregationDuplicate = GetDuplicateCodes(aggregationProduct).ToList();

            var parent = FindParent(report.GetAggregationData(pathAggregation[0]), "04640112140070ddXGrUN");
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
