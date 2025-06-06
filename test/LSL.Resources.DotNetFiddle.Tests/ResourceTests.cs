using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;

namespace LSL.Resources.DotNetFiddle.Tests;

public class ResourceTests
{
    [Test]
    public void JsonTestClassResource_ShouldBeTheExpectedValue()
    {
        var type = typeof(JsonTestClass);
        JsonSerializer.Deserialize(GetStream($".{type.FullName}.json"), type)
            .Should()
            .BeEquivalentTo(new JsonTestClass
            {
                Name = "Als",
                Age = 12
            });
    }

    [Test]
    public void JsonTestClassResourceCustomName_ShouldBeTheExpectedValue()
    {
        var type = typeof(JsonTestClass);
        JsonSerializer.Deserialize(GetStream($"other.json"), type)
            .Should()
            .BeEquivalentTo(new JsonTestClass
            {
                Name = "Als2",
                Age = 13
            });
    }

    [Test]
    public void JsonTestClassWithEnumResource_ShouldBeTheExpectedValue()
    {
        var type = typeof(JsonTestClassWithEnum);
        var settings = new JsonSerializerOptions();
        var name = $"{type.FullName}.json";
        settings.Converters.Add(new JsonStringEnumConverter());
        
        JsonSerializer.Deserialize(GetStream(name), type, settings)
            .Should()
            .BeEquivalentTo(new JsonTestClassWithEnum
            {
                Name = "OtherAls",
                Age = 10,
                Answer = YesOrNo.No
            });
    }

    [Test]
    public void StringContent_ShouldBeTheExpectedResult()
    {
        GetResource("text-file.txt")
            .Should()
            .Be("Text file");
    }    

    static string GetResource(string name)
    {
        using var stream = GetStream(name);
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    static Stream GetStream(string name)
    {
        var assembly = typeof(JsonTestClass).Assembly;
        var names = assembly.GetManifestResourceNames().OrderBy(n => n);
        var resourceName = names
            .Where(n => n.EndsWith(name))
            .First();

        return assembly.GetManifestResourceStream(resourceName);
    }
}