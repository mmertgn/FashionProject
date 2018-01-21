using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FashionStore_BLL.Services.Abstracts;

namespace FashionStore_BLL.Services.Concretes
{
    public class SeoUrlMaker : ISeoUrlMaker
    {
        public string MakeSlug(string input)
        {
            input = input.ToLower(new CultureInfo("tr-TR"));
            input = StripTags(input);
            input = RemoveAccents(input);
            input = Regex.Replace(input, "&.+?;", "");
            input = Regex.Replace(input, "[^.a-z0-9 _-]", "");
            input = Regex.Replace(input, @"\.|\s+", "-");
            input = Regex.Replace(input, "-+", "-");
            input = input.Trim('-');
            return input;
        }

        private string RemoveAccents(string input)
        {
            var normalized = input.Replace('ı', 'i').Normalize(NormalizationForm.FormKD);
            var array = new char[input.Length];
            var arrayIndex = 0;
            foreach (var c in normalized)
            {
                if (char.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;
                array[arrayIndex] = c;
                arrayIndex++;
            }
            return new string(array, 0, arrayIndex);
        }

        private string StripTags(string input)
        {
            var array = new char[input.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var @let in input)
            {
                if (@let != '<')
                {
                    if (@let == '>')
                    {
                        inside = false;
                        continue;
                    }
                }
                else
                {
                    inside = true;
                    continue;
                }

                if (inside) continue;
                array[arrayIndex] = @let;
                arrayIndex++;
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
