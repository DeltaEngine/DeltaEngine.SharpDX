using System;
using System.IO;
using System.Xml.Linq;

namespace DeltaEngine.Content.Xml
{
	public class ContentTypeIdentifier 
	{
		public static ContentType ExtensionToType(string fileName)
		{
			var extension = Path.GetExtension(fileName);
			switch (extension.ToLower())
			{
				case ".png":
				case ".jpg":
				case ".bmp":
				case ".tif":
					return ContentType.Image;
				case ".wav":
					return ContentType.Sound;
				case ".gif":
					return ContentType.JustStore;
				case ".mp3":
				case ".ogg":
				case ".wma":
					return ContentType.Music;
				case ".mp4":
				case ".avi":
				case ".wmv":
					return ContentType.Video;
				case ".xml":
					return DetermineTypeForXmlFile(XDocument.Load(fileName)); //ncrunch: no coverage
				case ".json":
					return ContentType.Json;
				case ".fbx":
				case ".obj":
				case ".dae":
					return ContentType.Model;
				case ".deltamesh":
					return ContentType.Mesh;
				case ".deltaparticle":
					return ContentType.ParticleEmitter;
				case ".deltashader":
					return ContentType.Shader;
				case ".deltamaterial":
					return ContentType.Material;
				case ".deltageometry":
					return ContentType.Geometry;
			}
			throw new UnsupportedContentFileFoundCannotParseType(extension);
		}

		public class UnsupportedContentFileFoundCannotParseType : Exception
		{
			public UnsupportedContentFileFoundCannotParseType(string extension)
				: base(extension) { }
		}

		public static ContentType DetermineTypeForXmlFile(XDocument xmlFile)
		{
			var rootName = xmlFile.Root.Name.ToString();
			if (rootName == "Font")
				return ContentType.Font;
			if (rootName == "InputCommand")
				return ContentType.InputCommand;
			if (rootName == "Level")
				return ContentType.Level;
			return ContentType.Xml;
		}
	}
}