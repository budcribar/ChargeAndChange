
// https://blog.jeremylikness.com/blog/azure-ad-secured-serverless-cosmosdb-from-blazor-webassembly/
// https://www.freecodecamp.org/news/how-to-implement-azure-serverless-with-blazor-web-assembly/


namespace CCWebSite.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    //using Microsoft.Azure.CosmosDB.BulkExecutor;
    //using Microsoft.Azure.CosmosDB.BulkExecutor.BulkImport;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;


    public class DocumentDBRepository<T> : IDocumentDBRepository<T> where T : class
    {

        private readonly string Endpoint = "https://charge-and-change.documents.azure.com:443/";
        private readonly string Key = "hKi3EPRIufbRmIFk5ehkAsp6OXyfUdLvEoHRhrv0ADVxIJyoPS3RA3JgqWRBhAYLTjoXkL9aZAOfvmMNl64SDw==";

        private readonly string readOnlyKey = "StKc4jyS25jiGdzt9ypXBBvqUBADyo59kljTc0lUISGwsBKVQ0gpMrkh9gmMOpXSRLNLfEjDymMLrY3BzuyuXA==";
        private readonly string DatabaseId = "chargeAndChange";
        private readonly string CollectionId;// = "bev";
        private readonly DocumentClient client;

        public DocumentDBRepository(string collectionId)
        {
            this.CollectionId = collectionId;
            this.client = new DocumentClient(new Uri(Endpoint), Key);
            try
            {
                CreateDatabaseIfNotExistsAsync().Wait();
                CreateCollectionIfNotExistsAsync().Wait();
            }
            catch (Exception) { }
        }

        public async Task<T?> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results.Count() == 0 ? null : results.First();
        }

        static internal DocumentCollection GetCollectionIfExists(DocumentClient client, string databaseName, string collectionName)
        {
           
            return client.CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(databaseName))
                .Where(c => c.Id == collectionName).AsEnumerable().FirstOrDefault();
        }

        //public async void CreateItemsAsync(T[] item)
        //{
        //    ConnectionPolicy ConnectionPolicy = new ConnectionPolicy
        //    {
        //        ConnectionMode = ConnectionMode.Direct,
        //        ConnectionProtocol = Protocol.Tcp
        //    };

        //    using (var client = new DocumentClient(new Uri(Endpoint), Key, ConnectionPolicy))
        //    {
        //        // Set retry options high during initialization (default values).
        //        client.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 30;
        //        client.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 9;

        //        DocumentCollection dataCollection = GetCollectionIfExists(client, DatabaseId, CollectionId);
        //        IBulkExecutor bulkExecutor = new BulkExecutor(client, dataCollection);
        //        await bulkExecutor.InitializeAsync();

        //        // Set retries to 0 to pass complete control to bulk executor.
        //        client.ConnectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = 0;
        //        client.ConnectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = 0;

        //        BulkImportResponse bulkImportResponse = null;
        //        long totalNumberOfDocumentsInserted = 0;
        //        double totalRequestUnitsConsumed = 0;
        //        double totalTimeTakenSec = 0;

        //        long numberOfDocumentsToGenerate = item.Length;
        //        long numberOfDocumentsPerBatch = 100;
        //        int numberOfBatches = (int)Math.Ceiling(((double)numberOfDocumentsToGenerate) / numberOfDocumentsPerBatch);


        //        var tokenSource = new CancellationTokenSource();
        //        var token = tokenSource.Token;
        //        var documentNumber = 0;

        //        for (int i = 0; i < numberOfBatches; i++)
        //        {
        //            // Generate JSON-serialized documents to import.

        //            List<string> documentsToImportInBatch = new List<string>();
        //            long prefix = i * numberOfDocumentsPerBatch;

        //            Trace.TraceInformation(String.Format("Generating {0} documents to import for batch {1}", numberOfDocumentsPerBatch, i));
        //            for (int j = 0; j < numberOfDocumentsPerBatch; j++)
        //            {
        //                if (documentNumber < item.Length)
        //                    documentsToImportInBatch.Add(item[documentNumber++].ToString());
        //            }

        //            // Invoke bulk import API.

        //            var tasks = new List<Task>
        //            {
        //                Task.Run(async () =>
        //                {
        //                    Trace.TraceInformation(String.Format("Executing bulk import for batch {0}", i));
        //                    do
        //                    {
        //                        try
        //                        {
        //                            bulkImportResponse = await bulkExecutor.BulkImportAsync(
        //                                documents: documentsToImportInBatch,
        //                                enableUpsert: false,
        //                                disableAutomaticIdGeneration: true,
        //                                maxConcurrencyPerPartitionKeyRange: null,
        //                                maxInMemorySortingBatchSize: null,
        //                                cancellationToken: token);
        //                        }
        //                        catch (DocumentClientException de)
        //                        {
        //                            Trace.TraceError("Document client exception: {0}", de);
        //                            break;
        //                        }
        //                        catch (Exception e)
        //                        {
        //                            Trace.TraceError("Exception: {0}", e);
        //                            break;
        //                        }
        //                    } while (bulkImportResponse.NumberOfDocumentsImported < documentsToImportInBatch.Count);

        //                    Trace.WriteLine(String.Format("\nSummary for batch {0}:", i));
        //                    Trace.WriteLine("--------------------------------------------------------------------- ");
        //                    Trace.WriteLine(String.Format("Inserted {0} docs @ {1} writes/s, {2} RU/s in {3} sec",
        //                        bulkImportResponse.NumberOfDocumentsImported,
        //                        Math.Round(bulkImportResponse.NumberOfDocumentsImported / bulkImportResponse.TotalTimeTaken.TotalSeconds),
        //                        Math.Round(bulkImportResponse.TotalRequestUnitsConsumed / bulkImportResponse.TotalTimeTaken.TotalSeconds),
        //                        bulkImportResponse.TotalTimeTaken.TotalSeconds));
        //                    Trace.WriteLine(String.Format("Average RU consumption per document: {0}",
        //                        (bulkImportResponse.TotalRequestUnitsConsumed / bulkImportResponse.NumberOfDocumentsImported)));
        //                    Trace.WriteLine("---------------------------------------------------------------------\n ");

        //                    totalNumberOfDocumentsInserted += bulkImportResponse.NumberOfDocumentsImported;
        //                    totalRequestUnitsConsumed += bulkImportResponse.TotalRequestUnitsConsumed;
        //                    totalTimeTakenSec += bulkImportResponse.TotalTimeTaken.TotalSeconds;
        //                },
        //            token)
        //            };

        //            await Task.WhenAll(tasks);
        //        }


        //    }


        //}
        public async Task<Document> CreateItemAsync(T item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id),new RequestOptions { PartitionKey = new PartitionKey(id) });
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId,   PartitionKey=new PartitionKeyDefinition {  Paths= new Collection<string> { "/id" } } },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}