using System;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Selenium.FX.Tests")]

namespace pbdq.Tests.Selenium.FX.Helpers
{
    internal class XPathBuilder
    {
        internal static string GetTagNamePart(string tagName, bool isLocalName = false)
        {
            if (tagName == null)
            {
                return isLocalName
                    ? throw new ArgumentNullException("Tag Name cannot be null.", (Exception) null)
                    : "*";
            }

            if (isLocalName)
                ValidateForNCName(tagName, "Tag Name");
            else
                ValidateForQName(tagName, "Tag Name");

            if (isLocalName)
                return $"*[local-name()='{tagName}']";

            if (IsReservedFunctionName(tagName))
                return $"*[name()='{tagName}']";

            return tagName;
        }

        internal static string GetAttributePart(string attibuteName, string attributeValue)
        {
            ValidateForQName(attibuteName, "Attribute Name");

            return $"[@{attibuteName}]";
        }

        #region Validation methods

        private static void ValidateForQName(string qName, string elementTypeName)
        {
            if (qName == null)
                throw new ArgumentNullException($"{elementTypeName} cannot be null.", (Exception) null);

            if (qName == string.Empty)
                throw new ArgumentException($"{elementTypeName} cannot be empty.", (Exception) null);

            var parts = qName.Split(':');
            if (parts.Length > 2)
                throw new FormatException($"{elementTypeName} cannot contain more than 1 colon.");
            if (parts[0] == string.Empty)
                throw new FormatException($"{elementTypeName} prefix cannot be empty.", null);
            if (parts.Length > 1 && parts[1] == string.Empty)
                throw new FormatException($"{elementTypeName} cannot be empty.", null);

            foreach (var part in parts)
                ValidateForNCName(part, elementTypeName);
        }

        private static void ValidateForNCName(string ncName, string elementTypeName)
        {
            if (ncName == null)
                throw new ArgumentNullException($"{elementTypeName} cannot be null.", (Exception) null);

            if (ncName == string.Empty)
                throw new ArgumentException($"{elementTypeName} cannot be empty.", (Exception) null);

            if (ncName.IndexOf(':') != -1)
                throw new ArgumentException($"{elementTypeName} contains invalid character “:”.", (Exception) null);

            for (var i = 0; i < ncName.Length; i++)
            {
                if (IsNameChar(ncName[i]) == false)
                    throw new ArgumentException($"{elementTypeName} contains invalid character “{ncName[i]}”.", (Exception) null);
                
                if (i == 0 && IsNameStartChar(ncName[i]) == false)
                    throw new ArgumentException($"{elementTypeName} cannot start with “{ncName[0]}”.", (Exception) null);
            }
        }

        private static readonly string[] ReservedFunctionNames =
        {
            "attribute", "comment", "document-node", "element", "empty-sequence", "if", "item", "node",
            "processing-instruction", "schema-attribute", "text", "typeswitch"
        };

        private static bool IsReservedFunctionName(string name)
        {
            var parts = name.Split(':');
            return parts.Any(part => ReservedFunctionNames.Contains(part));
        }

        #region Character validation methods

        /// <remarks>According to https://www.w3.org/TR/REC-xml/#NT-NameStartChar</remarks>
        private static bool IsNameStartChar(char character)
        {
            if (character == ':')
                return true;

            if (character >= 'A' && character <= 'Z')
                return true;

            if (character == '_')
                return true;

            if (character >= 'a' && character <= 'z')
                return true;

            if (character >= 0xC0 && character <= 0xD6)
                return true;

            if (character >= 0xD8 && character <= 0xF6)
                return true;

            if (character >= 0xF8 && character <= 0x2FF)
                return true;

            if (character >= 0x370 && character <= 0x37D)
                return true;

            if (character >= 0x37F && character <= 0x1FFF)
                return true;

            if (character >= 0x200C && character <= 0x200D)
                return true;

            if (character >= 0x2070 && character <= 0x218F)
                return true;

            if (character >= 0x2C00 && character <= 0x2FEF)
                return true;

            if (character >= 0x3001 && character <= 0xD7FF)
                return true;

            if (character >= 0xF900 && character <= 0xFDCF)
                return true;

            if (character >= 0xFDF0 && character <= 0xFFFD)
                return true;

            if (character >= 0x10000 && character <= 0xEFFFF)
                return true;

            return false;
        }

        /// <remarks>According to https://www.w3.org/TR/REC-xml/#NT-NameChar</remarks>
        private static bool IsNameChar(char character)
        {
            if (IsNameStartChar(character))
                return true;

            if (character == '-' || character == '.')
                return true;

            if (character >= '0' && character <= '9')
                return true;

            if (character == 0xB7)
                return true;

            if (character >= 0x0300 && character <= 0x036F)
                return true;

            if (character >= 0x203F && character <= 0x2040)
                return true;

            return false;
        }

        #endregion

        #endregion
    }
}