# Federation.Services.Core

Core functionality used for web scraping information based
on [Microsoft's OpenScraping](https://github.com/microsoft/openscraping-lib-csharp) JSON configs

### Overview

This library exposes an `HtmlParser<T>` class that can be used to parse extracted information from an HTML document. The
first step in creating an `HtmlParser<T>` is to define JSON configuration. This configuration should be written
according to Microsoft's [OpenScraping](https://github.com/microsoft/openscraping-lib-csharp).

For example, to extract a player's name from
their [uschess.org profile](https://www.uschess.org/msa/MbrDtlMain.php?13607491), the following JSON configuration could
be used.

```json
{
  "PlayerName": {
    "_xpath": "//font/b/text()",
    "_transformations": [
      {
        "_type": "RegexTransformation",
        "_regex": "[0-9]+: (.*)"
      },
      "HtmlDecodeTransformation",
      "RemoveExtraWhitespaceTransformation",
      "TrimTransformation"
    ]
  }
}
```

Next, create an `HtmlParser<T>` based on this JSON configuration.

```c#
var jsonString = "...";
var htmlParser = new HtmlParser<PlayerProfile>(jsonString);

public class PlayerProfile {
    public string? PlayerName { get; set; }
}
```

After setting up an instance, you can now parse HTML documents.

```c#
using var htmlStream = await httpClient.GetStreamAsync("https://www.uschess.org/msa/MbrDtlMain.php?13607491");
using var streamReader = new StreamReader(htmlStream);

PlayerProfile profile = htmlParser.Parse(await streamReader.ReadToEndAsync());
```
