using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class TagService
{
    private readonly string tagsFilePath;

    public TagService()
    {
        var baseDirectory = AppContext.BaseDirectory;
        tagsFilePath = Path.Combine(baseDirectory, "Data", "tags.json");
    }

    // Method to read all tags from the JSON file asynchronously
    public async Task<List<string>> ReadTagsAsync()
    {
        if (!File.Exists(tagsFilePath))
        {
            throw new FileNotFoundException($"The file {tagsFilePath} does not exist.");
        }

        var json = await File.ReadAllTextAsync(tagsFilePath); // Async file read
        var tagData = JsonConvert.DeserializeObject<TagData>(json);

        return tagData?.Tags ?? new List<string>();
    }

    // Method to add a new tag to the JSON file asynchronously
    public async Task AddTagAsync(string newTag)
    {
        if (string.IsNullOrWhiteSpace(newTag))
        {
            throw new ArgumentException("Tag cannot be null or empty.", nameof(newTag));
        }

        // Read the current tags asynchronously
        var tags = await ReadTagsAsync();

        // Check if the tag already exists (case insensitive)
        if (tags.Contains(newTag, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The tag '{newTag}' already exists.");
        }

        // Add the new tag
        tags.Add(newTag);

        // Write updated tags back to the JSON file asynchronously
        var tagData = new TagData { Tags = tags };
        var updatedJson = JsonConvert.SerializeObject(tagData, Formatting.Indented);

        // Async write operation
        await File.WriteAllTextAsync(tagsFilePath, updatedJson);
    }
}

// Class to map the JSON structure
public class TagData
{
    public List<string> Tags { get; set; }
}
