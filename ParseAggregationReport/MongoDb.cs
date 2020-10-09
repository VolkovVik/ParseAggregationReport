using System.Collections.Generic;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConsoleApp1
{
    // ReSharper disable once IdentifierTypo
    class MongoDb < T > 
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly MongoClient _client;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMongoDatabase _database;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMongoCollection< T > _collection;

        /// <summary>
        /// 
        /// </summary>
        public string NameDatabase { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string NameCollection { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private List<T> _list = new List< T >();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        public MongoDb(string database, string collection )
        {
            NameDatabase = database;
            NameCollection = collection;

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[ "MongoDb" ].ConnectionString;
                _client = new MongoClient( connectionString );
                _database = _client.GetDatabase( NameDatabase );
                _collection = _database.GetCollection< T >( collection );
            } catch ( MongoException exc )
            {
                
            }
        }

        /// <summary>
        /// Получить список баз данных сервера
        /// </summary>
        /// <returns></returns>
        public IEnumerable< BsonDocument > GetDatabases() => _client.ListDatabases().ToList();

        /// <summary>
        /// Получить список баз данных сервера
        /// </summary>
        /// <returns></returns>
        public IEnumerable< BsonDocument > GetCollection( string database = null ) =>
            string.IsNullOrWhiteSpace( database )
                ? _database.ListCollections().ToList()
                : _client.GetDatabase( database ).ListCollections().ToList();
        
        /// <summary>
        /// 
        /// </summary>
        public async void CreateAsync( T document ) {
            await _collection.InsertOneAsync( document );
        }

        /// <summary>
        /// 
        /// </summary>
        public async void CreateAsync( T[] document )
        {
            if ( document.Length == 0 ) return;
            try
            {
                await _collection.InsertManyAsync( document );
            } catch ( MongoException exc )
            {
                
            }
        }

        public List< T > Read()
        {
            //var filter = new BsonDocument();
            var filter = Builders< T >.Filter.Empty;
            return _collection.Find( filter ).ToList(); // ToListAsync()
        }

        public void Update( ObjectId id, T document )
        {
            var filter = Builders< T >.Filter.Eq( "Id", id );
            _collection.ReplaceOne( filter, document );
        }

        public void Delete( ObjectId id )
        {
            var deleteFilter = Builders< T >.Filter.Eq( "Id", id );
            _collection.DeleteOne( deleteFilter );
        }

        public void DropCollection() => _database.DropCollection( NameCollection );

        public void DropDatabase() => _client.DropDatabase( NameDatabase );
       
    }
}