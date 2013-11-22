using System;
using System.IO;
using System.Xml.Linq;
using DeltaEngine.Core;

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
				case ".atlas":
				case ".txt":
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
				case ".deltascene":
					return ContentType.Scene; //ncrunch: no coverage
			}
#if DEBUG
			Logger.Warning("Unknown content type, unable to proceed : " +
				Path.GetFileName(fileName));
			throw new UnsupportedContentFileFoundCannotParseType(extension);
#else
			return ContentType.JustStore;
#endif
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