using System.Collections.Immutable;
using System.Reflection;
using Newtonsoft.Json.Linq;
using OpenScraping;
using OpenScraping.Config;

namespace Federation.Services.Core;

public class HtmlParser<T> where T : new()
{
    // Cache off T's properties
    private static readonly ImmutableArray<PropertyInfo> PropertyInfos =
        typeof(T).GetProperties().Where(t => t.CanWrite).ToImmutableArray();

    private readonly ConfigSection configSection;
    private readonly StructuredDataExtractor dataExtractor;

    public HtmlParser(string jsonConfig)
    {
        configSection = StructuredDataConfig.ParseJsonString(jsonConfig);
        dataExtractor = new StructuredDataExtractor(configSection);
    }

    /// <summary>
    /// Parses <paramref name="html"/> and attempts to build an object of type <typeparamref name="T"/>
    /// whose property values are set based on the json config.
    /// </summary>
    /// <param name="html">The HTML to parse</param>
    /// <returns>An instance of type <typeparamref name="T"/> or null</returns>
    public T? Parse(string html)
    {
        JContainer container = dataExtractor.Extract(html);

        if (!container.HasValues) return default;

        // Create a new instance of type T and 
        var obj = Activator.CreateInstance<T>();

        // Dynamically retrieve and set the properties in type T with the values from the parsed html
        foreach (PropertyInfo property in PropertyInfos.Where(p => configSection.Children.ContainsKey(p.Name)))
        {
            JToken? propertyToken = container[property.Name];
            object? propertyValue = propertyToken.ToObject(property.PropertyType);

            property.SetValue(obj, propertyValue);
        }

        return obj;
    }

    /// <inheritdoc cref="Parse(string)"/>
    public T? Parse(Stream htmlStream)
    {
        using Stream stream = htmlStream;
        using StreamReader streamReader = new(stream);

        return Parse(streamReader.ReadToEnd());
    }
}