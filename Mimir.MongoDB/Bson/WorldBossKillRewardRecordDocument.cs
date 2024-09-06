using Lib9c.Models.States;
using Libplanet.Crypto;

namespace Mimir.MongoDB.Bson;

public record WorldBossKillRewardRecordDocument(
    Address Address,
    Address AvatarAddress,
    WorldBossKillRewardRecord Object
) : MimirBsonDocument(Address) { }
