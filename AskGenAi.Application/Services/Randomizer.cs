using System.Security.Cryptography;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Application.Services;

public class Randomizer : IRandomizer
{
    public string GenerateKey()
    {
        var key = new byte[32]; // 256 bits
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(key);
        }

        return Convert.ToBase64String(key);
    }
}