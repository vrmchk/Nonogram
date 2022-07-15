using System;
using System.IO;
using System.Text.Json;

namespace Nonogram.Models;

internal class FieldSaver
{
    private const string pathToSavedField = @"SavedField.json";
    private readonly Field _field;

    public FieldSaver(Field field)
    {
        _field = field;
    }

    public void Save()
    {
        using StreamWriter writer = new StreamWriter(pathToSavedField, append: false);
        writer.Write(JsonSerializer.Serialize(_field.AsSerializable()));
    }

    public IFieldGenerator GetExistingFieldGenerator()
    {
        IFieldGenerator generator;
        try
        {
            using StreamReader reader = new StreamReader(pathToSavedField);
            string fileContent = reader.ReadToEnd();
            if (string.IsNullOrEmpty(fileContent))
                throw new InvalidOperationException("Game hasn't been saved");

            generator = JsonSerializer.Deserialize<SerializableField>(fileContent) ??
                        throw new InvalidOperationException("File data were incorrect");
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException("Saving file doesn't exist");
        }
        finally
        {
            RewriteFile();
        }

        return generator;
    }

    private void RewriteFile() => File.Create(pathToSavedField).Close();
}