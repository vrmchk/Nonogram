using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Nonogram.Models;

internal class FieldSaver
{
    private readonly string _pathToSavedField;
    private readonly Field _field;

    public FieldSaver(Field field)
    {
        _pathToSavedField = @"SavedField";
        _field = field;
    }

    public void Save()
    {
        using (StreamWriter writer = new StreamWriter(_pathToSavedField, append: false))
        {
            var (cells, blockContent, hintsLeft) = _field;
            SerializableField serializableField = new SerializableField(cells, blockContent, hintsLeft);
            writer.Write(JsonSerializer.Serialize(serializableField));
        }
    }

    public void LoadExistingGame()
    {
        using (StreamReader reader = new StreamReader(_pathToSavedField))
        {
            string fileText = reader.ReadToEnd();
            if (fileText == string.Empty)
                throw new InvalidOperationException("Game wasn't saved before");

            SerializableField? fromJson = JsonSerializer.Deserialize<SerializableField>(fileText);
            if (fromJson == null)
                throw new NullReferenceException("Unable to deserialize a file");

            _field.LoadExistingGame(fromJson.Cells, fromJson.BlocksContent, fromJson.HintsLeft);
        }

        ClearExistingGame();
    }

    private void ClearExistingGame() => File.Create(_pathToSavedField).Close();
}