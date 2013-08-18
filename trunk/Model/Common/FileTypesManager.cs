using System;
using System.Collections.Generic;
using System.Reflection;

namespace Model.Common
{
    class FileTypesManager
    {
        public struct FileTypeInfo
        {
            public string[] Extensions;
            public string Description;
            public string MimeType;
        }

        private static IDictionary<FileType, FileTypeInfo> TypesInfo = new Dictionary<FileType, FileTypeInfo>() 
        { 
            { FileType.Undefined, new FileTypeInfo{ Extensions = new string[] {string.Empty}, Description = string.Empty, MimeType = string.Empty} },

            { FileType.Png, new FileTypeInfo{ 
                Extensions = new string[] {"png"}, 
                Description = "Portable Network Grafics", 
                MimeType = "image/png"} },
            { FileType.Gif, new FileTypeInfo{ 
                Extensions = new string[] {"gif"}, 
                Description = "Graphics Interchange Format", 
                MimeType = "image/gif"} },
            { FileType.Jpeg, new FileTypeInfo{ 
                Extensions = new string[] {"jpeg", "jpg"}, 
                Description = "Joint Photographic Experts Group", 
                MimeType = "image/jpeg"} },

            { FileType.Flv, new FileTypeInfo{ 
                Extensions = new string[] {"flv"}, 
                Description = "Flash Video", 
                MimeType ="video/x-flv"} },
            { FileType.Avi, new FileTypeInfo{ 
                Extensions = new string[] {"avi"}, 
                Description = "Audio Video Interleav", 
                MimeType="video/avi"} },
            { FileType.Mpg, new FileTypeInfo{ 
                Extensions = new string[] {"mpg"}, 
                Description = "MPEG", 
                MimeType ="video/mpeg"} },           

            { FileType.Pdf, new FileTypeInfo{ 
                Extensions = new string[] {"pdf"}, 
                Description = "Portable Document Format", 
                MimeType ="application/pdf"} },          
            { FileType.Svg, new FileTypeInfo{ 
                Extensions = new string[] {"svg"}, 
                Description = "Scalable Vector Graphics", 
                MimeType ="image/svg+xml"} },        

            { FileType.Doc, new FileTypeInfo{ 
                Extensions = new string[] {"doc"}, 
                Description = "MS Word 2003, 2007", 
                MimeType ="application/msword"} },
            { FileType.Docx, new FileTypeInfo{ 
                Extensions = new string[] {"docx"}, 
                Description = "Ms Word 2007, 2013", 
                MimeType ="application/msword"} },
            { FileType.Xls, new FileTypeInfo{ 
                Extensions = new string[] {"xls"}, 
                Description = "MS Word 2003, 2007", 
                MimeType ="application/msword"} },
            { FileType.Xlsx, new FileTypeInfo{ 
                Extensions = new string[] {"xlsx"}, 
                Description = "MS Word 2007, 2013", 
                MimeType ="application/msword"} },

            { FileType.Exe, new FileTypeInfo{ 
                Extensions = new string[] {"exe"}, 
                Description = "Executable file", 
                MimeType ="application/exe"} },

            { FileType.Zip, new FileTypeInfo{ 
                Extensions = new string[] {"zip", "zipx"}, 
                Description = "ZIP archive", 
                MimeType ="application/zip"} },
        };

        private readonly FileType[] managedFileTypes;

        public FileTypesManager(FileType[] managedFileTypes)
        {
            this.managedFileTypes = managedFileTypes;
        }

        public static FileType GetFileTypeFromExtension(string extension)
        {
            extension = extension.ToUpper();
            foreach (KeyValuePair<FileType, FileTypeInfo> typeInfo in TypesInfo)
            {
                if (ContainExtension(typeInfo.Value, extension))
                {
                    return typeInfo.Key;
                }
            }
            return FileType.Undefined;
        }

        public static FileTypeInfo GetFileTypeInfo(FileType fileType)
        {
            return TypesInfo[fileType];
        }

        public bool IsValid(string key)
        {
            key = key.ToUpper();
            foreach (KeyValuePair<FileType, FileTypeInfo> typeInfo in TypesInfo)
            {
                if ((IsManagedFileType(typeInfo.Key)) &&
                    ContainExtension(typeInfo.Value, key))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ContainExtension(FileTypeInfo info, string target)
        {
            foreach (string extension in info.Extensions)
            {
                if (extension.ToUpper().Equals(target))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsManagedFileType(FileType fileType)
        {
            foreach (FileType managedFileType in managedFileTypes)
            {
                if (managedFileType == fileType)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetFilesFilterString()
        {
            List<string> builder = new List<string>();
            foreach (FileType type in managedFileTypes)
            {
                builder.Add(string.Format("{0}({1})|{1}", TypesInfo[type].Description, GetExtensionsString(TypesInfo[type])));
            }
            return string.Join("|", builder);
        }

        private string GetExtensionsString(FileTypeInfo info)
        {
            List<string> builder = new List<string>();
            foreach (string extension in info.Extensions)
            {
                builder.Add(string.Format("*.{0}", extension));
            }

            return string.Join(";", builder);
        }

        private static PropertyInfo GetBinaryProperty(Type objType, string propName)
        {
            PropertyInfo property = objType.GetProperty(propName);
            if (property == null)
            {
                throw new ApplicationException(string.Format("У объекта [{0}] не найдено свойство [{1}]", objType.GetType().FullName, propName));
            }

            if (property.PropertyType != typeof(Byte[]))
            {
                throw new ApplicationException(string.Format("Свойство [{0}] должно быть типа byte[]", propName));
            }
            return property;
        }

        public static FileTypesManager CreateForProperty(Type objType, string propName)
        {
            PropertyInfo property = GetBinaryProperty(objType, propName);
            ByteContentAttribute attribute = property.GetCustomAttribute<ByteContentAttribute>();
            if (attribute == null)
            {
                throw new ApplicationException(string.Format("Атрибут [ByteContentAttribute] не найден у свойства [{0}]", propName));
            }

            return new FileTypesManager(attribute.FileTypes);
        }

    }
}
