using HashidsNet;

namespace GameStore.API.Services;

public class HashidService
{
    private readonly Hashids _hashids;

    public HashidService(string salt, int minHashLength)
    {
        _hashids = new Hashids(salt, minHashLength);
    }

    public string Encode(int id) => _hashids.Encode(id);

    public int Decode(string hashid)
    {
        var result = _hashids.Decode(hashid);
        return result.Length > 0 ? result[0] : -1;
    }
}