using System;
using System.Runtime.CompilerServices;
using System.Text;
using pbdq.Tests.Selenium.FX.Helpers.Validators;

[assembly: InternalsVisibleTo("Selenium.FX.Tests")]

namespace pbdq.Tests.Selenium.FX.Helpers
{
    internal static class XPathBuilder
    {
        private static readonly Exception NullException = null;

        internal static string GetTagNamePart(string tagName, bool isLocalName = false)
        {
            if (tagName == null)
            {
                return isLocalName
                    ? throw new ArgumentNullException("Tag Name cannot be null.", NullException)
                    : "*";
            }

            if (isLocalName)
                XPathValidator.ValidateNCName(tagName, "Tag Name");
            else
                XPathValidator.ValidateQName(tagName, "Tag Name");

            if (isLocalName)
                return $"*[local-name()='{tagName}']";

            if (XPathValidator.IsReservedFunctionName(tagName))
                return $"*[name()='{tagName}']";

            return tagName;
        }

        internal static string GetAttributePart(string attributeName, string attributeValue)
        {
            if (attributeName != null)
                XPathValidator.ValidateQName(attributeName, "Attribute Name");

            var namePart = attributeName != null
                ? $"@{attributeName}"
                : "@*";

            string valuePart = null;
            if (attributeValue != null)
            {
                var encodedValue = EncodeAttributeValue(attributeValue);
                valuePart = $"='{encodedValue}'";
            }

            return $"[{namePart}{valuePart}]";
        }

        // from: https://devhints.io/xpath#class-check
        internal static string GetCssClassPart(string cssClass)
        {
            if (cssClass == null)
                throw new ArgumentNullException("CSS Class Name cannot be null.", NullException);

            CssValidator.ValidateClassName(cssClass);

            var encodedValue = EncodeAttributeValue(cssClass);

            return $"[contains(concat(' ',normalize-space(@class),' '),' {encodedValue} ')]";
        }

        internal static string GetTextPart(string text, bool isPartial = false)
        {
            if (text == null)
                return "[text()]";

            var encodedText = EncodeAttributeValue(text);

            return isPartial
                ? $"[contains(text(),'{encodedText}')]"
                : $"[text()='{encodedText}']";
        }

        #region Encode attribute value

        // TODO: update according to https://referencesource.microsoft.com/#System.Xml/System/Xml/Core/XmlTextEncoder.cs : Write() (for full support of Unicode)
        private static string EncodeAttributeValue(string attributeValue)
        {
            if (attributeValue == null)
                throw new ArgumentNullException("Value to decode cannot be null.", NullException);

            var decodedOutput = new StringBuilder();
            foreach (var character in attributeValue)
            {
                string decodedChar;

                switch (character)
                {
                    case (char) 0xA:
                    case (char) 0xD:
                        decodedChar = CreateXmlEntityFromCharacter(character);
                        break;
                    case '<':
                        decodedChar = CreateXmlEntityFromName("lt");
                        break;
                    case '>':
                        decodedChar = CreateXmlEntityFromName("gt");
                        break;
                    case '&':
                        decodedChar = CreateXmlEntityFromName("amp");
                        break;
                    case '\'':
                        decodedChar = CreateXmlEntityFromName("apos");
                        break;
                    default:
                        decodedChar = character < 0x20 || character > 0xFFFD
                            ? CreateXmlEntityFromCharacter(character)
                            : character.ToString();
                        break;
                }

                decodedOutput.Append(decodedChar);
            }

            return decodedOutput.ToString();
        }

        private static string CreateXmlEntityFromCharacter(char character)
        {
            var hexCode = ((int) character).ToString("X");
            return $"&#x{hexCode};";
        }

        private static string CreateXmlEntityFromName(string name)
        {
            return $"&{name};";
        }

        #endregion
    }
}