using Lib9c.GraphQL.Enums;
using Libplanet.Crypto;
using Mimir.Exceptions;
using Mimir.Models.Assets;
using Mimir.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mimir.Repositories;

public class AllRuneRepository(MongoDBCollectionService mongoDbCollectionService)
    : BaseRepository<BsonDocument>(mongoDbCollectionService)
{
    public List<Rune> GetRunes(PlanetName planetName, Address avatarAddress)
    {
        var collection = GetCollection(planetName);
        var filter = Builders<BsonDocument>.Filter.Eq("Address", avatarAddress.ToHex());
        var document = collection.Find(filter).FirstOrDefault();
        if (document is null)
        {
            throw new DocumentNotFoundInMongoCollectionException(
                $"AllRune document not found in {collection.CollectionNamespace.CollectionName} collection.");
        }

        try
        {
            var runesDoc = document["State"]["Object"]["Runes"].AsBsonDocument;
            return runesDoc
                .Select(rune => rune.Value.AsBsonDocument)
                .Select(runeValue => new Rune(runeValue))
                .ToList();
        }
        catch (KeyNotFoundException e)
        {
            throw new UnexpectedTypeOfBsonValueException("Invalid key used in AllRune document", e);
        }
    }

    protected override string GetCollectionName() => "all_rune";
}
