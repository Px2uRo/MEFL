using Microsoft.UI.Xaml;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace MEFL.Contract
{
    internal class GameInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
#if WPF
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
#endif
            var game = value as GameInfoBase;
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(game.Version));
                writer.WriteValue(game.Version);
                writer.WritePropertyName(nameof(game.Name));
                writer.WriteValue(game.Name);
                writer.WritePropertyName(nameof(game.JVMArgs));
                writer.WriteValue(game.JVMArgs);
                writer.WritePropertyName(nameof(game.LibrariesPath));
                writer.WriteValue(game.LibrariesPath);
                writer.WritePropertyName(nameof(game.NativeLibrariesPath));
                writer.WriteValue(game.NativeLibrariesPath);
                writer.WritePropertyName(nameof(game.ClassPaths));
                writer.WriteValue($"共 {game.ClassPaths.Count} 个");
                writer.WritePropertyName(nameof(game.MainClassName));
                writer.WriteValue(game.MainClassName);
                writer.WritePropertyName(nameof(game.VersionType));
                writer.WriteValue(game.VersionType);
                writer.WritePropertyName(nameof(game.GameTypeFriendlyName));
                writer.WriteValue(game.GameTypeFriendlyName);
                writer.WritePropertyName(nameof(game.GameFolder));
                writer.WriteValue(game.GameFolder);
                writer.WritePropertyName(nameof(game.AssetsIndexName));
                writer.WriteValue(game.AssetsIndexName);
                writer.WriteEndObject();
                writer.Flush();
#if WPF
        }));
#endif
        }
    }
}